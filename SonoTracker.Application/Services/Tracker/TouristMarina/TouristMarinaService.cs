using LinqKit;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Application.Services.Lookup.City;
using SonoTracker.Application.Services.Lookup.Town;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.Town;
using SonoTracker.Common.DTO.Tracker.TouristMarina;
using SonoTracker.Common.DTO.Tracker.TouristMarina.Parameters;
using SonoTracker.Common.Helpers;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.TouristMarina
{
    public class MarinaOrganizationService(
        IServiceBaseParameter<Entities.Tracker.TouristMarina> businessBaseParameter,
        ICityService cityService) 
        : BaseService<Domain.Entities.Tracker.TouristMarina, AddTouristMarinaDto, EditTouristMarinaDto, TouristMarinaDto, string, string>(businessBaseParameter), ITouristMarinaService
    {
        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            Entities.Tracker.TouristMarina entity = await UnitOfWork.Repository
                                            .FirstOrDefaultAsync(x => x.Id.Equals(id.ToString()), include: src => src
                                            .Include(t => t.City)
                                            .Include(x => x.GeoPoint),
                                             cancellationToken: cancellationToken);

            EditTouristMarinaDto mapped = Mapper.Map<Entities.Tracker.TouristMarina, EditTouristMarinaDto>(entity);
            
            return ResponseResult.PostResult(result: mapped, status: HttpStatusCode.OK, exception: null,
                                             message: HttpStatusCode.OK.ToString());
        }

        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            Entities.Tracker.TouristMarina entity = await UnitOfWork.Repository
                                    .FirstOrDefaultAsync(x => x.Id.Equals(id.ToString()), include: src => src
                                    .Include(t => t.City)
                                    .Include(x => x.GeoPoint), 
                                    cancellationToken: cancellationToken);

            TouristMarinaDto mapped = Mapper.Map<Entities.Tracker.TouristMarina, TouristMarinaDto>(entity);

            return ResponseResult.PostResult(result: mapped, status: HttpStatusCode.OK, exception: null,
                                             message: HttpStatusCode.OK.ToString());
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.TouristMarina, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            IEnumerable<Entities.Tracker.TouristMarina> entity = await UnitOfWork.Repository
                                         .GetAllAsync(include: src => src
                                         .Include(t => t.City)
                                         .Include(x => x.GeoPoint), 
                                          cancellationToken: cancellationToken);
            
            string governorateId = IsSuperAdmin() ? null : GetGovernorateIdFromClaims();

            IEnumerable<Entities.Tracker.TouristMarina> filteredEntities = IsSuperAdmin()
                ? entity
                : entity.Where(e => !e.IsDeleted && (string.IsNullOrWhiteSpace(governorateId) || e.GovernorateId == governorateId));

            IEnumerable<TouristMarinaDto> mapped = Mapper.Map<IEnumerable<Entities.Tracker.TouristMarina>, IEnumerable<TouristMarinaDto>>(filteredEntities);
            
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<IFinalResult> GetAllFilterAsync(TouristMarinaFilter filter, CancellationToken cancellationToken = default)
        {
            IEnumerable<Entities.Tracker.TouristMarina> entity = await UnitOfWork.Repository
                                .FindAsync(predicate: PredicateBuilderFunction(filter), cancellationToken: cancellationToken);

            IEnumerable<TouristMarinaDto> data = Mapper.Map<IEnumerable<Entities.Tracker.TouristMarina>, IEnumerable<TouristMarinaDto>>(entity.Where(x => x.IsDeleted != true));

            return ResponseResult.PostResult(result: data, status: HttpStatusCode.OK, exception: null,
                                             message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<TouristMarinaFilter> filter, CancellationToken cancellationToken = default)
        {
            bool isSuperAdmin = IsSuperAdmin();

            TouristMarinaFilter touristMarinaFilter = filter?.Filter ?? new TouristMarinaFilter();
            
            string governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();
            
            if (!isSuperAdmin)
                touristMarinaFilter.IsDeleted = false;

            int limit = filter.PageSize;

            int offset = --filter.PageNumber * filter.PageSize;

            (int Count, IEnumerable<Entities.Tracker.TouristMarina> Result) query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(touristMarinaFilter, governorateId), pageNumber: offset,
                pageSize: limit, filter.OrderByValue,
                include: src => src
             .Include(t => t.GeoPoint)
             .Include(x => x.City),
                cancellationToken: cancellationToken);


            var items = isSuperAdmin ? query.Result : query.Result.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Tracker.TouristMarina>, IEnumerable<TouristMarinaDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var isSuperAdmin = IsSuperAdmin();
            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(
                    predicate: predicate,
                    pageNumber: offset,
                    pageSize: limit,
                    cancellationToken: cancellationToken);

            var items = isSuperAdmin ? query.Item2 : query.Item2.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Tracker.TouristMarina>, IEnumerable<TouristMarinaDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }


        static Expression<Func<Entities.Tracker.TouristMarina, bool>> PredicateBuilderFunction(TouristMarinaFilter filter, string governorateId = null)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TouristMarina>(x => x.IsDeleted == filter.IsDeleted);

            if (!string.IsNullOrEmpty(filter.CityId))
            {
                predicate = predicate.And(x => x.CityId == filter.CityId);
            }
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                predicate = predicate.And(x => x.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrWhiteSpace(filter.Code))
            {
                predicate = predicate.And(x => x.Code.Contains(filter.Code));
            }

            if (!string.IsNullOrWhiteSpace(governorateId))
            {
                predicate = predicate.And(x => x.GovernorateId == governorateId);
            }

            return predicate;
        }
        static Expression<Func<Entities.Tracker.TouristMarina, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TouristMarina>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.Code.Contains(filter.SearchCriteria));
                //  predicate = predicate.Or(b => b.Name.Contains(filter.SearchCriteria));
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

      
        public override async Task<IFinalResult> AddAsync(AddTouristMarinaDto model, CancellationToken cancellationToken = default)
        {
            try
            {
                await AssignMarinaCodeFromCityAsync(model, cancellationToken);

                bool isSuperAdmin = IsSuperAdmin();
                
                string govId = GetGovernorateIdFromClaims();

                IEnumerable<Entities.Tracker.TouristMarina> existingForDup = isSuperAdmin || string.IsNullOrWhiteSpace(govId)
                    ? await UnitOfWork.Repository.FindAsync(
                        predicate: x => x.IsDeleted != true,
                        disableTracking: true,
                        cancellationToken: cancellationToken)
                    : await UnitOfWork.Repository.FindAsync(
                        predicate: x => x.GovernorateId == govId && x.IsDeleted != true,
                        disableTracking: true,
                        cancellationToken: cancellationToken);

                if (LookupDuplicateGuard.HasFuzzyNameDuplicate(existingForDup, m => m.Name, m => string.Empty, model.Name, string.Empty))
                    return ResponseResult.PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                                                     message: MessagesConstants.Existed);

                Entities.Tracker.TouristMarina entity = Mapper.Map<Entities.Tracker.TouristMarina>(model);

                await UnitOfWork.Repository.AddAsync(entity, cancellationToken);
                
                int affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);

                if (affectedRows < 0)
                    return ResponseResult.PostResult(result :false, status: HttpStatusCode.BadRequest, exception: null, 
                                                     message: MessagesConstants.AddError);

                return ResponseResult.PostResult(result: entity.Id, status: HttpStatusCode.Created, exception: null,
                                                 message: MessagesConstants.AddSuccess);
            }
            catch (Exception ex)
            {
                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, exception: ex,
                                                 message: MessagesConstants.AddError + ex.Message);
            }
        }

        public override async Task<IFinalResult> UpdateAsync(AddTouristMarinaDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                bool isSuperAdmin = IsSuperAdmin();

                string govId = GetGovernorateIdFromClaims();

                IEnumerable<Entities.Tracker.TouristMarina> existingForDup = isSuperAdmin || string.IsNullOrWhiteSpace(govId)
                    ? await UnitOfWork.Repository.FindAsync(
                        predicate: x => x.Id != dto.Id && x.IsDeleted != true,
                        disableTracking: true,
                        cancellationToken: cancellationToken)
                    : await UnitOfWork.Repository.FindAsync(
                        predicate: x => x.GovernorateId == govId && x.Id != dto.Id && x.IsDeleted != true,
                        disableTracking: true,
                        cancellationToken: cancellationToken);

                if (LookupDuplicateGuard.HasFuzzyNameDuplicate(existingForDup, m => m.Name, m => string.Empty, dto.Name, string.Empty))
                    return ResponseResult.PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                        message: MessagesConstants.Existed);

                Entities.Tracker.TouristMarina entityToUpdate = await UnitOfWork.Repository.GetAsync(cancellationToken, dto.Id);

                Entities.Tracker.TouristMarina newEntity = Mapper.Map(dto, entityToUpdate);

                if (IsSuperAdmin())
                {
                    if (entityToUpdate.IsDeleted)
                        newEntity.IsDeleted = false;
                }

                UnitOfWork.Repository.Update(entityToUpdate, newEntity);
                
                int affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);

                if (affectedRows < 0)
                    return ResponseResult.PostResult(result: false, status: HttpStatusCode.BadRequest, exception: null,
                                                     message: MessagesConstants.UpdateError);

                return ResponseResult.PostResult(result: true, status: HttpStatusCode.Accepted, exception: null,
                                                 message: MessagesConstants.UpdateSuccess);
            }
            catch (Exception ex)
            {
                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, exception: ex,
                                                 message: MessagesConstants.UpdateError + ex.Message);
            }
        }

        private async Task AssignMarinaCodeFromCityAsync(AddTouristMarinaDto model, CancellationToken cancellationToken)
        {
            IFinalResult city = await cityService.GetByIdAsync(model.CityId, cancellationToken);
            
            if (city.Data is not TownDto townDto)
                return;

            var cityCode = townDto.Code ?? string.Empty;

            var filter = new TouristMarinaFilter
            {
                CityId = model.CityId,
                IsDeleted = false
            };

            IFinalResult exist = await GetAllFilterAsync(filter, cancellationToken);

            ICollection<TouristMarinaDto> existDataCollection = exist.Data as ICollection<TouristMarinaDto>;

            int nextSeq = 1;
            
            if (existDataCollection is { Count: > 0 })
            {
                TouristMarinaDto lastItem = existDataCollection
                                .Where(x => x.Code != null &&
                                    x.Code.Length >= cityCode.Length + 3 &&
                                    x.Code.StartsWith(cityCode, StringComparison.Ordinal))
                                .OrderByDescending(x => x.Code)
                                .FirstOrDefault();

                if (lastItem?.Code != null &&
                    int.TryParse(lastItem.Code.AsSpan(lastItem.Code.Length - 3), out var number))
                    nextSeq = number + 1;
            }

            model.Code = cityCode + nextSeq.ToString("D3");
        }
    }
}
