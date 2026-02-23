using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.GovernorateGeoPoint.Parameters;
using SonoTracker.Common.DTO.Tracker.GovernorateGeoPoint;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Common.DTO.Lookup.Town;
using SonoTracker.Common.DTO.Tracker.Governorate;

namespace SonoTracker.Application.Services.Tracker.GovernorateGeoPoint
{
    public class GovernorateGeoPointService : BaseService<Entities.Tracker.GovernorateGeoPoint, AddGovernorateGeoPointDto, EditGovernorateGeoPointDto, GovernorateGeoPointDto, Guid, Guid?>, IGovernorateGeoPointService
    {

        public GovernorateGeoPointService(IServiceBaseParameter<Entities.Tracker.GovernorateGeoPoint> businessBaseParameter) : base(businessBaseParameter)
        {


        }
        public override async Task<IFinalResult> GetByIdForEditAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id.ToString()),
                include: src => src
                .Include(t => t.GeoPoint)
               .Include(x => x.Governorate)
                );
            var mapped = Mapper.Map<Domain.Entities.Tracker.GovernorateGeoPoint, EditGovernorateGeoPointDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id.ToString()),
                include: src => src
                .Include(t => t.GeoPoint)
               .Include(x => x.Governorate));
            var mapped = Mapper.Map<Domain.Entities.Tracker.GovernorateGeoPoint, GovernorateGeoPointDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.GovernorateGeoPoint, bool>> predicate = null)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync();
            var filteredEntities = entity.Where(e => !e.IsDeleted);
            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.GovernorateGeoPoint>, IEnumerable<GovernorateGeoPointDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<GovernorateGeoPointFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter),
                pageNumber: offset, pageSize: limit,
                filter.OrderByValue, include: src => src
              .Include(t => t.Governorate)
              .Include(x => x.GeoPoint)
                );

            var data = Mapper.Map<IEnumerable<Entities.Tracker.GovernorateGeoPoint>, IEnumerable<GovernorateGeoPointDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
      

        static Expression<Func<Entities.Tracker.GovernorateGeoPoint, bool>> PredicateBuilderFunction(GovernorateGeoPointFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.GovernorateGeoPoint>(x => x.IsDeleted == filter.IsDeleted);
           
            if (filter.GovernorateId.HasValue)
            {
                predicate = predicate.And(x => x.GovernorateId== filter.GovernorateId.Value);
            }
            if (filter.GeoPointId.HasValue)
            {
                predicate = predicate.And(x => x.GeoPointId == filter.GeoPointId.Value);
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
