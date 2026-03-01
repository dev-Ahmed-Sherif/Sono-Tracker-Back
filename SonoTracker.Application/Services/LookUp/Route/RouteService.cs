using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Application.Services.LookUp.Route;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.Route;
using SonoTracker.Common.DTO.Lookup.Route.Parameters;
using SonoTracker.Common.DTO.Lookup.Town;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Lookup.Route
{
    public class RouteService(IServiceBaseParameter<Entities.Lookups.Route> businessBaseParameter) : BaseService<Entities.Lookups.Route, AddRouteDto, EditRouteDto, RouteDto, Guid, Guid?>(businessBaseParameter), IRouteService
    {
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Lookups.Route, bool>> predicate = null)
        {
            // Retrieve all entities
            var entity = await UnitOfWork.Repository.GetAllAsync(disableTracking: disableTracking);

            // Filter out deleted records
            var filteredEntities = entity.Where(e => !e.IsDeleted);

            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Lookups.Route>, IEnumerable<RouteDto>>(filteredEntities);

            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<RouteFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue);

            var data = Mapper.Map<IEnumerable<Entities.Lookups.Route>, IEnumerable<RouteDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
  
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter)
        {

            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit);

            var data = Mapper.Map<IEnumerable<Entities.Lookups.Route>, IEnumerable<RouteDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }

        static Expression<Func<Entities.Lookups.Route, bool>> PredicateBuilderFunction(RouteFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Lookups.Route>(x => x.IsDeleted == filter.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter.NameAr))
            {
                predicate = predicate.And(x => x.NameAr.Contains(filter.NameAr));
            }
            if (!string.IsNullOrWhiteSpace(filter.NameEn))
            {
                predicate = predicate.And(x => x.NameEn.Contains(filter.NameEn));
            }

            return predicate;
        }
 
        static Expression<Func<Entities.Lookups.Route, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Lookups.Route>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.NameAr.Contains(filter.SearchCriteria));
                predicate = predicate.Or(b => b.NameEn.Contains(filter.SearchCriteria));
            }
            return predicate;
        }

        public override async Task<IFinalResult> AddAsync(AddRouteDto model)
        {
            try
            {
                var IsExisted = await UnitOfWork.Repository.Any(x =>
                                    x.NameAr == model.NameAr &&
                                    x.NameEn == model.NameEn &&
                                    x.IsDeleted != true);

                if (IsExisted)
                    return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict,
                                                message: MessagesConstants.Existed);

                var entity = Mapper.Map<Entities.Lookups.Route>(model);

                var data = await GetAllAsync();

                var dataCollection = data.Data as ICollection<RouteDto>;

                if (dataCollection?.Count > 0)
                {
                    if (int.TryParse(dataCollection.OrderByDescending(o => o.Code).FirstOrDefault().Code.
                        AsSpan(dataCollection.OrderByDescending(o => o.Code).FirstOrDefault().Code.Length - 1),
                        out int num))
                    {
                        int newCode = ++num;
                        entity.Code = newCode.ToString("D2");
                    }
                }
                else
                {
                    entity.Code = "01";
                }

                //SetEntityCreatedBaseProperties(entity);
                await UnitOfWork.Repository.AddAsync(entity);
                var affectedRows = await UnitOfWork.SaveChangesAsync();

                if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.AddError);

                return ResponseResult.PostResult(result: entity.Id, HttpStatusCode.Created, null, MessagesConstants.AddSuccess);
            }
            catch (Exception e)
            {
                //_logger.LogError($"{MessagesConstants.AddError}-{nameof(AddAsync)}");
                //_logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                throw;
            }

        }

        public override async Task<IFinalResult> UpdateAsync(AddRouteDto model)
        {
            var IsExisted = await UnitOfWork.Repository.Any(x =>
                                   x.NameAr == model.NameAr &&
                                   x.NameEn == model.NameEn &&
                                   x.Id != model.Id &&
                                   x.IsDeleted != true);

            if (IsExisted)
                return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict, message: MessagesConstants.Existed);

            Domain.Entities.Lookups.Route entityToUpdate = await UnitOfWork.Repository.GetAsync(model.Id);

            var entity = Mapper.Map(model, entityToUpdate);

            UnitOfWork.Repository.Update(entityToUpdate, entity);

            //SetEntityModifiedBaseProperties(entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync();

            if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.UpdateError);

            return ResponseResult.PostResult(true, HttpStatusCode.OK, null, MessagesConstants.UpdateSuccess);
        }
    }

}
