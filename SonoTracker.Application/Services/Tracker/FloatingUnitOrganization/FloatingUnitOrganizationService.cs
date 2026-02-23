using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.FloatingUnit;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization.Parameters;
using SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization;
using Microsoft.EntityFrameworkCore;

namespace SonoTracker.Application.Services.Tracker.FloatingUnitOrganization
{
    public class FloatingUnitOrganizationService : BaseService<Entities.Tracker.FloatingUnitOrganization, AddFloatingUnitOrganizationDto, EditFloatingUnitOrganizationDto, FloatingUnitOrganizationDto, Guid, Guid?>, IFloatingUnitOrganizationService
    {

        public FloatingUnitOrganizationService(IServiceBaseParameter<Entities.Tracker.FloatingUnitOrganization> businessBaseParameter) : base(businessBaseParameter)
        {


        }
        public override async Task<IFinalResult> GetByIdForEditAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id.ToString()),
                include: src => src
                .Include(t => t.FloatingUnit)
                .Include(t => t.Organization));
            var mapped = Mapper.Map<Domain.Entities.Tracker.FloatingUnitOrganization, EditFloatingUnitOrganizationDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id.ToString()),
                include: src => src
                .Include(t => t.FloatingUnit)
                .Include(t => t.Organization));
            var mapped = Mapper.Map<Domain.Entities.Tracker.FloatingUnitOrganization, FloatingUnitOrganizationDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
       
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.FloatingUnitOrganization, bool>> predicate = null)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
                .Include(t => t.FloatingUnit)
                .Include(t => t.Organization));
            var filteredEntities = entity.Where(e => !e.IsDeleted);
            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.FloatingUnitOrganization>, IEnumerable<FloatingUnitOrganizationDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<FloatingUnitOrganizationFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue,
                 include: src => src
                .Include(t => t.FloatingUnit)
                .Include(t => t.Organization));

            var data = Mapper.Map<IEnumerable<Entities.Tracker.FloatingUnitOrganization>, IEnumerable<FloatingUnitOrganizationDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }

        static Expression<Func<Entities.Tracker.FloatingUnitOrganization, bool>> PredicateBuilderFunction(FloatingUnitOrganizationFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.FloatingUnitOrganization>(x => x.IsDeleted == false);

            if (filter.OrganizationId.HasValue)
            {
                predicate = predicate.And(x => x.OrganizationId == filter.OrganizationId.Value);
            }
            if (filter.OrganizationType.HasValue)
            {
                predicate = predicate.And(x => x.Organization.OrganizationTypeId == filter.OrganizationType);
            }
            if (filter.FloatingUnitId.HasValue)
            {
                predicate = predicate.And(x => x.FloatingUnitId == filter.FloatingUnitId.Value);
            }

            return predicate;
        }

        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var entitiesToDelete = await UnitOfWork.Repository.FindAsync(d => ids.Contains(d.Id));

            UnitOfWork.Repository.RemoveRange(entitiesToDelete);

            var rows = await UnitOfWork.SaveChangesAsync();

            return ResponseResult.PostResult(result: rows, status: HttpStatusCode.NoContent, message: MessagesConstants.DeleteSuccess);
        }

    }
}
