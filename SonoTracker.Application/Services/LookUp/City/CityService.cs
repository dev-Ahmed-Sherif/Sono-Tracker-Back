using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.City;
using SonoTracker.Common.DTO.Lookup.City.Parameters;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Lookup.City
{
    public class CityService(IServiceBaseParameter<Domain.Entities.Lookups.City> businessBaseParameter) : BaseService<Domain.Entities.Lookups.City, AddCityDto, EditCityDto, CityDto, string, string>(businessBaseParameter), ICityService
    {
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Lookups.City, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            IEnumerable<Domain.Entities.Lookups.City> entities = await UnitOfWork.Repository.GetAllAsync(disableTracking: disableTracking, cancellationToken: cancellationToken);

            var governorateId = IsSuperAdmin() ? null : GetGovernorateIdFromClaims();
            var filtered = IsSuperAdmin()
                ? (entities ?? Enumerable.Empty<Domain.Entities.Lookups.City>())
                : (entities?.Where(e => !e.IsDeleted && (string.IsNullOrWhiteSpace(governorateId) || e.GovernorateId == governorateId)) ?? Enumerable.Empty<Domain.Entities.Lookups.City>());
            IEnumerable<CityDto> mapped = Mapper.Map<IEnumerable<Domain.Entities.Lookups.City>, IEnumerable<CityDto>>(filtered);

            return ResponseResult.PostResult(result: mapped, status: HttpStatusCode.OK, exception: null,
                                             message: HttpStatusCode.OK.ToString());
        }
       
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<CityFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var cityFilter = filter?.Filter ?? new CityFilter();
            var governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();

            if (!isSuperAdmin)
                cityFilter.IsDeleted = false;

            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            (int Count, IEnumerable<Domain.Entities.Lookups.City> Result) = await UnitOfWork.Repository.FindPagedAsync(
                predicate: PredicateBuilderFunction(cityFilter, governorateId),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                cancellationToken: cancellationToken);

            var filteredResult = isSuperAdmin
                ? (Result ?? Enumerable.Empty<Domain.Entities.Lookups.City>())
                : (Result?.Where(x => x.IsDeleted != true) ?? Enumerable.Empty<Domain.Entities.Lookups.City>());
            var data = Mapper.Map<IEnumerable<Domain.Entities.Lookups.City>, IEnumerable<CityDto>>(filteredResult);

            return new PagingResult(filter.PageNumber, filter.PageSize, Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
       
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            if (!isSuperAdmin)
                predicate = predicate.And(x => x.IsDeleted != true);

            if (!string.IsNullOrWhiteSpace(governorateId))
                predicate = predicate.And(x => x.GovernorateId == governorateId);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Domain.Entities.Lookups.City>, IEnumerable<CityDto>>(query.Result);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }
       
        static Expression<Func<Domain.Entities.Lookups.City, bool>> PredicateBuilderFunction(CityFilter filter, string governorateId)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Lookups.City>(x => x.IsDeleted == filter.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter.NameAr))
            {
                predicate = predicate.And(x => x.NameAr.Contains(filter.NameAr));
            }
            if (!string.IsNullOrWhiteSpace(filter.NameEn))
            {
                predicate = predicate.And(x => x.NameEn.Contains(filter.NameEn));
            }
            if (!string.IsNullOrWhiteSpace(governorateId))
            {
                predicate = predicate.And(x => x.GovernorateId == governorateId);
            }

            return predicate;
        }
      
        static Expression<Func<Domain.Entities.Lookups.City, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Lookups.City>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.NameAr.Contains(filter.SearchCriteria));
                predicate = predicate.Or(b => b.NameEn.Contains(filter.SearchCriteria));
                
            }
            return predicate;
        }

        public override async Task<IFinalResult> AddAsync(AddCityDto model, CancellationToken cancellationToken = default)
        {
            try
            {
                var IsExisted = await UnitOfWork.Repository.Any(x =>
                    (x.NameAr == model.NameAr || x.NameEn == model.NameEn) && !x.IsDeleted, cancellationToken);

                if (IsExisted)
                    return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                        message: MessagesConstants.Existed);

                var entity = Mapper.Map<AddCityDto, Entities.Lookups.City>(model);

                IFinalResult lastEntity = await GetLastRecordAsync(cancellationToken);

                if (lastEntity.Data != null)
                {
                    if (lastEntity.Data is CityDto cityDto)
                    {
                        if (int.TryParse(cityDto.Code.AsSpan(cityDto.Code.Length - 2), out int num))
                        {
                            ++num;
                            entity.Code = num.ToString("D2");
                        }
                    }
                }
                else
                {
                    entity.Code = "01";
                }

                await UnitOfWork.Repository.AddAsync(entity, cancellationToken);
                var affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);

                if (affectedRows <= 0)
                    return ResponseResult.PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                        message: MessagesConstants.AddError);

                return ResponseResult.PostResult(result: entity.Id, status: HttpStatusCode.Created, exception: null,
                    message: MessagesConstants.AddSuccess);
            }
            catch (Exception e)
            {
                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, exception: e,
                    message: MessagesConstants.AddError + e.Message);
            }
        }

        public override async Task<IFinalResult> UpdateAsync(AddCityDto model, CancellationToken cancellationToken = default)
        {
            var IsExisted = await UnitOfWork.Repository.Any(x =>
                (x.NameAr == model.NameAr || x.NameEn == model.NameEn) && x.Id != model.Id && x.IsDeleted != true, cancellationToken);

            if (IsExisted)
                return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                    message: MessagesConstants.Existed);

            Entities.Lookups.City entityToUpdate = await UnitOfWork.Repository.GetAsync(cancellationToken, model.Id);

            var entity = Mapper.Map(model, entityToUpdate);

            if (IsSuperAdmin())
            {
                if (entityToUpdate.IsDeleted)
                    entity.IsDeleted = false;
            }

            UnitOfWork.Repository.Update(entityToUpdate, entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);

            if (affectedRows <= 0)
                return ResponseResult.PostResult(result: false, status: HttpStatusCode.BadRequest, exception: null,
                    message: MessagesConstants.UpdateError);

            return ResponseResult.PostResult(result: true, status: HttpStatusCode.OK, exception: null,
                message: MessagesConstants.UpdateSuccess);
        }
    }
}
