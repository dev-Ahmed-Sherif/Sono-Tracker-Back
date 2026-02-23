using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.MarinaTrip.Parameters;
using SonoTracker.Common.DTO.Tracker.MarinaTrip;
using SonoTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace SonoTracker.Application.Services.Tracker.MarinaTrip
{
    public class MarinaTripService : BaseService<Entities.Tracker.MarinaTrip, Common.DTO.Tracker.MarinaTrip.AddMarinaTripDto, EditMarinaTripDto, MarinaTripDto, Guid, Guid?>, IMarinaTripService
    {

        public MarinaTripService(IServiceBaseParameter<Entities.Tracker.MarinaTrip> businessBaseParameter) : base(businessBaseParameter)
        {


        }
        public override async Task<IFinalResult> GetByIdForEditAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id.ToString()),
                include: src => src
                .Include(t => t.TouristMarina)
               .Include(x => x.TripInformation)
                );
            var mapped = Mapper.Map<Domain.Entities.Tracker.MarinaTrip, EditMarinaTripDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id.ToString()),
                include: src => src
               .Include(t => t.TouristMarina)
              .Include(x => x.TripInformation)
              .ThenInclude(x => x.FloatingUnit));
            var mapped = Mapper.Map<Domain.Entities.Tracker.MarinaTrip, Common.DTO.Tracker.MarinaTrip.MarinaTripDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.MarinaTrip, bool>> predicate = null)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
               .Include(t => t.TouristMarina)
              .Include(x => x.TripInformation)
              .ThenInclude(x => x.FloatingUnit));
            var filteredEntities = entity.Where(e => !e.IsDeleted);
            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.MarinaTrip>, IEnumerable<MarinaTripDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<Common.DTO.Tracker.MarinaTrip.Parameters.MarinaTripFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter),
                pageNumber: offset, pageSize: limit,
                filter.OrderByValue, include: src => src
                .Include(t => t.TouristMarina)
              .Include(x => x.TripInformation)
              .ThenInclude(x => x.FloatingUnit)
                );

            var data = Mapper.Map<IEnumerable<Entities.Tracker.MarinaTrip>, IEnumerable<MarinaTripDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }


        static Expression<Func<Entities.Tracker.MarinaTrip, bool>> PredicateBuilderFunction(MarinaTripFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.MarinaTrip>(x => x.IsDeleted == filter.IsDeleted);

            if (filter.TouristMarinaId.HasValue)
            {
                predicate = predicate.And(x => x.TouristMarinaId == filter.TouristMarinaId.Value);
            }
            if (filter.TripInformationId.HasValue)
            {
                predicate = predicate.And(x => x.TripInformationId == filter.TripInformationId.Value);
            }

            if (filter.FloatingUnitId.HasValue)
            {
                predicate = predicate.And(x => x.TripInformation.FloatingUnitId == filter.FloatingUnitId.Value);
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
