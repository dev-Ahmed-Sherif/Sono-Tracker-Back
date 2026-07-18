using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FuzzySharp;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripPassenger;
using SonoTracker.Common.DTO.Tracker.TripPassenger.Parameters;
using SonoTracker.Domain;

namespace SonoTracker.Application.Services.Tracker.TripPassenger
{
    public class TripPassengerService(IServiceBaseParameter<Entities.Tracker.TripPassenger> businessBaseParameter)
        : BaseService<Entities.Tracker.TripPassenger, AddTripPassengerDto, EditTripPassengerDto, TripPassengerDto, string, string>(businessBaseParameter), ITripPassengerService
    {
        private static string NormalizeText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var parts = value.Trim().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(' ', parts).ToLowerInvariant();
        }

        private static string NormalizeIdentity(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return new string(value.Where(char.IsDigit).ToArray());
        }

        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation)
                    .Include(t => t.Governorate),
                cancellationToken: cancellationToken);
            var mapped = Mapper.Map<Entities.Tracker.TripPassenger, EditTripPassengerDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation)
                    .Include(t => t.Governorate),
                cancellationToken: cancellationToken);

            var mapped = Mapper.Map<Entities.Tracker.TripPassenger, TripPassengerDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Entities.Tracker.TripPassenger, bool>> predicate = null, CancellationToken cancellationToken = default)
            => await GetAllAsync(tripInformationId: null, cancellationToken);

        public async Task<IFinalResult> GetAllAsync(string? tripInformationId, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var filter = new TripPassengerFilter
            {
                TripInformationId = tripInformationId,
                IsDeleted = false
            };
            var governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();

            var entity = await UnitOfWork.Repository.FindAsync(
                predicate: PredicateBuilderFunction(filter, governorateId, includeDeleted: isSuperAdmin),
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation)
                    .Include(t => t.Governorate),
                cancellationToken: cancellationToken);

            var mapped = Mapper.Map<IEnumerable<Entities.Tracker.TripPassenger>, IEnumerable<TripPassengerDto>>(entity);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<TripPassengerFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var tripPassengerFilter = filter?.Filter ?? new TripPassengerFilter();
            var governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();

            if (!isSuperAdmin)
                tripPassengerFilter.IsDeleted = false;

            var limit = filter.PageSize;
            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(
                predicate: PredicateBuilderFunction(tripPassengerFilter, governorateId, includeDeleted: false),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation)
                    .Include(t => t.Governorate),
                cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripPassenger>, IEnumerable<TripPassengerDto>>(query.Item2);

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
                    .Include(t => t.TripInformation)
                    .Include(t => t.Governorate),
                cancellationToken: cancellationToken);

            var items = isSuperAdmin ? query.Item2 : query.Item2.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripPassenger>, IEnumerable<TripPassengerDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }

        public async Task<IFinalResult> GetAllFilterAsync(TripPassengerFilter filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();

            var entities = await UnitOfWork.Repository.FindAsync(
                predicate: PredicateBuilderFunction(filter, governorateId, includeDeleted: isSuperAdmin),
                cancellationToken: cancellationToken,
                include: src => src.Include(t => t.Nationality)
                    .Include(t => t.TripInformation)
                    .Include(t => t.Governorate));

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripPassenger>, IEnumerable<TripPassengerDto>>(entities.Where(x => x.IsDeleted != true));

            return ResponseResult.PostResult(data, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        static Expression<Func<Entities.Tracker.TripPassenger, bool>> PredicateBuilderFunction(TripPassengerFilter filter, string governorateId = null, bool includeDeleted = false)
        {
            var predicate = includeDeleted
                ? PredicateBuilder.New<Entities.Tracker.TripPassenger>(true)
                : PredicateBuilder.New<Entities.Tracker.TripPassenger>(x => x.IsDeleted == filter.IsDeleted);

            if (!string.IsNullOrWhiteSpace(filter.Name))
                predicate = predicate.And(x => x.Name.Contains(filter.Name));

            if (!string.IsNullOrWhiteSpace(filter.Mobile))
                predicate = predicate.And(x => x.Mobile.Contains(filter.Mobile));

            if (!string.IsNullOrWhiteSpace(filter.Email))
                predicate = predicate.And(x => x.Email.Contains(filter.Email));

            if (!string.IsNullOrWhiteSpace(filter.Identity))
                predicate = predicate.And(x => x.Identity.Contains(filter.Identity));

            if (filter.Gender.HasValue)
                predicate = predicate.And(x => x.Gender == filter.Gender.Value);

            if (filter.IDType.HasValue)
                predicate = predicate.And(x => x.IDType == filter.IDType.Value);

            if (!string.IsNullOrEmpty(filter.TripInformationId))
                predicate = predicate.And(x => x.TripInformationId == filter.TripInformationId);

            if (!string.IsNullOrEmpty(filter.NationalityId))
                predicate = predicate.And(x => x.NationalityId == filter.NationalityId);

            if (!string.IsNullOrEmpty(filter.GovernorateId))
                predicate = predicate.And(x => x.GovernorateId == filter.GovernorateId);

            if (!string.IsNullOrWhiteSpace(governorateId))
                predicate = predicate.And(x => x.GovernorateId == governorateId);

            return predicate;
        }

        static Expression<Func<Entities.Tracker.TripPassenger, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TripPassenger>(x => !x.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
                predicate = predicate.And(b => b.Name.Contains(filter.SearchCriteria));

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

        public override async Task<IFinalResult> AddAsync(AddTripPassengerDto model, CancellationToken cancellationToken = default)
        {
            try
            {
                if (await HasDuplicateAsync(model.TripInformationId, model.Name, model.Identity, excludeId: null, cancellationToken))
                    return ResponseResult.PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                        message: MessagesConstants.Existed);

                var entity = Mapper.Map<Entities.Tracker.TripPassenger>(model);
                entity.GovernorateId = GetGovernorateIdFromClaims();
                entity.IsDeleted = false;

                await UnitOfWork.Repository.AddAsync(entity, cancellationToken);

                int affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);

                if (affectedRows <= 0)
                    return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.AddError);

                return ResponseResult.PostResult(result: entity.Id, HttpStatusCode.Created, null, MessagesConstants.AddSuccess);
            }
            catch (Exception ex)
            {
                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, exception: ex,
                    message: MessagesConstants.AddError + ex.Message);
            }
        }

        public override async Task<IFinalResult> UpdateAsync(AddTripPassengerDto model, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityToUpdate = await UnitOfWork.Repository.GetAsync(cancellationToken, model.Id);

                if (await HasDuplicateAsync(model.TripInformationId, model.Name, model.Identity, excludeId: model.Id, cancellationToken))
                    return ResponseResult.PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                        message: MessagesConstants.Existed);

                var entity = Mapper.Map(model, entityToUpdate);
                entity.GovernorateId = GetGovernorateIdFromClaims();

                if (IsSuperAdmin() && entityToUpdate.IsDeleted)
                    entity.IsDeleted = false;

                UnitOfWork.Repository.Update(entityToUpdate, entity);

                int affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);

                if (affectedRows < 0)
                    return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.UpdateError);

                return ResponseResult.PostResult(true, HttpStatusCode.OK, null, MessagesConstants.UpdateSuccess);
            }
            catch (Exception ex)
            {
                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, exception: ex,
                    message: MessagesConstants.UpdateError + ex.Message);
            }
        }

        async Task<bool> HasDuplicateAsync(string tripInformationId, string name, string identity, string excludeId, CancellationToken cancellationToken)
        {
            var existingPassengers = await UnitOfWork.Repository.FindAsync(
                predicate: x => x.TripInformationId == tripInformationId && (excludeId == null || x.Id != excludeId),
                disableTracking: true,
                cancellationToken: cancellationToken);

            var normalizedName = NormalizeText(name);
            var normalizedIdentity = NormalizeIdentity(identity);
            const int nameThreshold = 90;
            const int identityThreshold = 90;

            var nameDuplicate = existingPassengers.Any(x =>
                !string.IsNullOrWhiteSpace(x.Name) &&
                Fuzz.TokenSetRatio(normalizedName, NormalizeText(x.Name)) >= nameThreshold);

            var identityExactDuplicate = existingPassengers.Any(x =>
                !string.IsNullOrWhiteSpace(x.Identity) &&
                string.Equals(x.Identity, identity, StringComparison.Ordinal));

            var identityFuzzyDuplicate = existingPassengers.Any(x =>
                !string.IsNullOrWhiteSpace(x.Identity) &&
                Fuzz.Ratio(normalizedIdentity, NormalizeIdentity(x.Identity)) >= identityThreshold);

            return nameDuplicate || identityExactDuplicate || identityFuzzyDuplicate;
        }
    }
}
