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
using SonoTracker.Domain;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Common.DTO.Tracker.TripGeo;
using SonoTracker.Common.DTO.Tracker.TripGeo.Parameters;

namespace SonoTracker.Application.Services.Tracker.TripGeo
{
    public class TripGeoService(IServiceBaseParameter<Entities.Tracker.TripGeo> businessBaseParameter) : BaseService<Entities.Tracker.TripGeo, AddTripGeoDto, EditTripGeoDto, TripGeoDto, Guid, Guid?>(businessBaseParameter), ITripGeoService
    {
        public override async Task<IFinalResult> GetByIdForEditAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id.ToString()),
                include: src => src.Include(t => t.GeoPoint)
                .Include(t => t.TripInformation).ThenInclude(t => t.FloatingUnit)
                );
            var mapped = Mapper.Map<Entities.Tracker.TripGeo, EditTripGeoDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id.ToString()),
                include: src => src.Include(t => t.GeoPoint)
                .Include(t => t.TripInformation).ThenInclude(t => t.FloatingUnit));

            var mapped = Mapper.Map<Entities.Tracker.TripGeo, EditTripGeoDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public async Task<IFinalResult> GetLastByFloatingUnitIdAsync(object id)
        {
            var entity = await UnitOfWork.Repository
                    .LastOrDefaultAsync(x => x.TripInformation.FloatingUnitId == Guid.Parse(id.ToString()),
                            //orderBy: q => q.OrderByDescending(d => d.),
                            include: src => src
                                       .Include(t => t.GeoPoint)
                                       .Include(t => t.TripInformation)
                                       .ThenInclude(t => t.FloatingUnit));

            var mapped = Mapper.Map<Entities.Tracker.TripGeo, EditTripGeoDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Entities.Tracker.TripGeo, bool>> predicate = null)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src.Include(t => t.GeoPoint)
             .Include(t => t.TripInformation)
             .ThenInclude(t => t.FloatingUnit)
            );
            var filteredEntities = entity.Where(e => e.IsDeleted != true);
            var mapped = Mapper.Map<IEnumerable<Entities.Tracker.TripGeo>, IEnumerable<TripGeoDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<TripGeoFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter),
                pageNumber: offset, pageSize: limit,
                filter.OrderByValue,
                include: src => src.Include(t => t.GeoPoint)
                .Include(t => t.TripInformation).ThenInclude(t => t.FloatingUnit)
                );

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripGeo>, IEnumerable<TripGeoDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter)
        {

            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripGeo>, IEnumerable<TripGeoDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }
        public async Task<IFinalResult> GetAllFilterAsync(TripGeoFilter filter)
        {
            var entity = await UnitOfWork.Repository.FindAsync(predicate: PredicateBuilderFunction(filter));

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripGeo>, IEnumerable<TripGeoDto>>(entity.Where(x => x.IsDeleted != true));

            return ResponseResult.PostResult(data, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        static Expression<Func<Entities.Tracker.TripGeo, bool>> PredicateBuilderFunction(TripGeoFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TripGeo>(x => x.IsDeleted == filter.IsDeleted);

            if (filter.TripInformationId.HasValue)
            {
                predicate = predicate.And(x => x.TripInformationId == filter.TripInformationId.Value);
            }
            //if (filter.SartDate.HasValue)
            //{
            //    predicate = predicate.And(x => x.SartDate.Date >= filter.SartDate.Value.Date);
            //}
            //if (filter.EndDate.HasValue)
            //{
            //    predicate = predicate.And(x => x.EndDate<= filter.EndDate.Value.Date);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.Code))
            //{
            //    predicate = predicate.And(x => x.Code.Contains(filter.Code));
            //}

            return predicate;
        }
        static Expression<Func<Entities.Tracker.TripGeo, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TripGeo>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                //predicate = predicate.And(b => b.Code.Contains(filter.SearchCriteria));
                //  predicate = predicate.Or(b => b.Name.Contains(filter.SearchCriteria));
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
