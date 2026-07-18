using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripNationality;
using SonoTracker.Common.DTO.Tracker.TripNationality.Parameters;
using SonoTracker.Domain;

namespace SonoTracker.Application.Services.Tracker.TripNationality
{
    public class TripNationalityService(IServiceBaseParameter<Entities.Tracker.TripNationality> businessBaseParameter)
        : BaseService<Entities.Tracker.TripNationality, AddTripNationalityDto, EditTripNationalityDto, TripNationalityDto, string, string>(businessBaseParameter), ITripNationalityService
    {
        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation),
                cancellationToken: cancellationToken);
            var mapped = Mapper.Map<Entities.Tracker.TripNationality, EditTripNationalityDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation),
                cancellationToken: cancellationToken);

            var mapped = Mapper.Map<Entities.Tracker.TripNationality, TripNationalityDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Entities.Tracker.TripNationality, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation),
                cancellationToken: cancellationToken);

            var filteredEntities = IsSuperAdmin()
                ? entity
                : entity.Where(e => e.IsDeleted != true);

            var mapped = Mapper.Map<IEnumerable<Entities.Tracker.TripNationality>, IEnumerable<TripNationalityDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<TripNationalityFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var tripNationalityFilter = filter?.Filter ?? new TripNationalityFilter();
            if (!isSuperAdmin)
                tripNationalityFilter.IsDeleted = false;

            var limit = filter.PageSize;
            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(
                predicate: PredicateBuilderFunction(tripNationalityFilter),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation),
                cancellationToken: cancellationToken);

            var items = isSuperAdmin ? query.Item2 : query.Item2.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripNationality>, IEnumerable<TripNationalityDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }

        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var limit = filter.PageSize;
            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(
                predicate: predicate,
                pageNumber: offset,
                pageSize: limit,
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation),
                cancellationToken: cancellationToken);

            var items = isSuperAdmin ? query.Item2 : query.Item2.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripNationality>, IEnumerable<TripNationalityDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }

        public async Task<IFinalResult> GetAllFilterAsync(TripNationalityFilter filter, CancellationToken cancellationToken = default)
        {
            var entities = await UnitOfWork.Repository.FindAsync(
                predicate: PredicateBuilderFunction(filter),
                cancellationToken: cancellationToken,
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation));

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripNationality>, IEnumerable<TripNationalityDto>>(entities.Where(x => x.IsDeleted != true));

            return ResponseResult.PostResult(data, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        static Expression<Func<Entities.Tracker.TripNationality, bool>> PredicateBuilderFunction(TripNationalityFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TripNationality>(x => x.IsDeleted == filter.IsDeleted);

            if (!string.IsNullOrEmpty(filter.TripInformationId))
            {
                predicate = predicate.And(x => x.TripInformationId == filter.TripInformationId);
            }

            if (!string.IsNullOrEmpty(filter.NationalityId))
            {
                predicate = predicate.And(x => x.NationalityId == filter.NationalityId);
            }

            return predicate;
        }

        static Expression<Func<Entities.Tracker.TripNationality, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TripNationality>(x => !x.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.Nationality.NameAr.Contains(filter.SearchCriteria)
                    || b.Nationality.NameEn.Contains(filter.SearchCriteria)
                    || b.Nationality.Code.Contains(filter.SearchCriteria));
            }
            return predicate;
        }

        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var idsList = ids.ToList();
            var entitiesToDelete = await UnitOfWork.Repository.FindAsync(d => idsList.Contains(d.Id), cancellationToken: cancellationToken);

            UnitOfWork.Repository.RemoveRange(entitiesToDelete);

            var rows = await UnitOfWork.SaveChangesAsync(cancellationToken);

            return ResponseResult.PostResult(result: rows, status: HttpStatusCode.NoContent, message: MessagesConstants.DeleteSuccess);
        }
    }
}
