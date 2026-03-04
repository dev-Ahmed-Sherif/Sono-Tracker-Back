using LinqKit;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.Town;
using SonoTracker.Common.DTO.Lookup.Town.Parameters;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Lookup.Town
{
    public class TownService(IServiceBaseParameter<Domain.Entities.Lookups.Town> businessBaseParameter) : BaseService<Domain.Entities.Lookups.Town, AddTownDto, EditTownDto, TownDto, string, string>(businessBaseParameter), ITownService
    {
        public override async Task<IFinalResult> GetByIdForEditAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id.Equals(id),
                include: src => src
                .Include(t => t.City)
                .ThenInclude(x => x.Governorate));

            var mapped = Mapper.Map<Domain.Entities.Lookups.Town, EditTownDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id.Equals(id),
                include: src => src
                .Include(t => t.City)
                .ThenInclude(x => x.Governorate));
            var mapped = Mapper.Map<Domain.Entities.Lookups.Town, TownDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Entities.Lookups.Town, bool>> predicate = null)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
                                                    .Include(t => t.City)
                                                    .ThenInclude(x => x.Governorate) ,
                                                    disableTracking: disableTracking);
            var filteredEntities = entity.Where(e => !e.IsDeleted);

            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Lookups.Town>, IEnumerable<TownDto>>(filteredEntities);

            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<TownFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync
                (predicate: PredicateBuilderFunction(filter.Filter),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                include: src => src
                               .Include(t => t.City)
                               .ThenInclude(x=>x.Governorate));

            var data = Mapper.Map<IEnumerable<Domain.Entities.Lookups.Town>, IEnumerable<TownDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter)
        {

            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit);

            var data = Mapper.Map<IEnumerable<Domain.Entities.Lookups.Town>, IEnumerable<TownDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }
        static Expression<Func<Domain.Entities.Lookups.Town, bool>> PredicateBuilderFunction(TownFilter filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Lookups.Town>(x => x.IsDeleted == filter.IsDeleted);
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
        static Expression<Func<Domain.Entities.Lookups.Town, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Lookups.Town>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.NameAr.Contains(filter.SearchCriteria));
                predicate = predicate.Or(b => b.NameEn.Contains(filter.SearchCriteria));
            }
            return predicate;
        }
        public override async Task<IFinalResult> AddAsync(AddTownDto model)
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

                var entity = Mapper.Map<Entities.Lookups.Town>(model);

                var data = await GetAllAsync();

                var dataCollection = data.Data as ICollection<TownDto>;

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

        public override async Task<IFinalResult> UpdateAsync(AddTownDto model)
        {
            //var nameEnRegex = new Regex("^[a-zA-Z]+$");

            //var nameArRegex = new Regex("^[\u0600-\u06FF\\s]+$");

            //if (nameEnRegex.Match(model.NameEn) == Match.Empty ||
            //    nameArRegex.Match(model.NameAr) == Match.Empty)
            //{
            //    return new ResponseResult().PostResult(result: false, status: HttpStatusCode.BadRequest,
            //                                message: "NameAr should Arabic and NameEn should be English and No Numbers allowed");
            //}

            var IsExisted = await UnitOfWork.Repository.Any(x =>
                                   x.NameAr == model.NameAr &&
                                   x.NameEn == model.NameEn &&
                                   x.Id != model.Id &&
                                   x.IsDeleted != true);

            if (IsExisted)
                return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict, message: MessagesConstants.Existed);

            Domain.Entities.Lookups.Town entityToUpdate = await UnitOfWork.Repository.GetAsync(model.Id);

            var entity = Mapper.Map(model, entityToUpdate);

            UnitOfWork.Repository.Update(entityToUpdate, entity);

            //SetEntityModifiedBaseProperties(entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync();

            if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.UpdateError);

            return ResponseResult.PostResult(true, HttpStatusCode.OK, null, MessagesConstants.UpdateSuccess);
        }

    }
}
