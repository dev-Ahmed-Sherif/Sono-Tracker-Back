using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.AccidentType;
using SonoTracker.Common.DTO.Lookup.AccidentType.Parameters;
using SonoTracker.Common.DTO.Lookup.City;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.LookUp.AccidentType
{
    public class AccidentTypeService(IServiceBaseParameter<Entities.Lookups.AccidentType> businessBaseParameter) : BaseService<Entities.Lookups.AccidentType, AddAccidentTypeDto, EditAccidentTypeDto, AccidentTypeDto, string, string>(businessBaseParameter), IAccidentTypeService
    {
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Lookups.AccidentType, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            IEnumerable<Domain.Entities.Lookups.AccidentType> entities = await UnitOfWork.Repository.GetAllAsync(disableTracking: disableTracking, cancellationToken: cancellationToken);

            var filtered = entities?.Where(e => !e.IsDeleted) ?? Enumerable.Empty<Domain.Entities.Lookups.AccidentType>();
            IEnumerable<AccidentTypeDto> mapped = Mapper.Map<IEnumerable<Domain.Entities.Lookups.AccidentType>, IEnumerable<AccidentTypeDto>>(filtered);

            return ResponseResult.PostResult(result: mapped, status: HttpStatusCode.OK, exception: null,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<AccidentTypeFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            (int Count, IEnumerable<Entities.Lookups.AccidentType> Result) = await UnitOfWork.Repository.FindPagedAsync(
                predicate: PredicateBuilderFunction(filter.Filter),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                cancellationToken: cancellationToken);

            var filteredResult = Result?.Where(x => x.IsDeleted != true) ?? Enumerable.Empty<Entities.Lookups.AccidentType>();
            var data = Mapper.Map<IEnumerable<Entities.Lookups.AccidentType>, IEnumerable<AccidentTypeDto>>(filteredResult);

            return new PagingResult(filter.PageNumber, filter.PageSize, Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Lookups.AccidentType>, IEnumerable<AccidentTypeDto>>(query.Result.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        static Expression<Func<Entities.Lookups.AccidentType, bool>> PredicateBuilderFunction(AccidentTypeFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Lookups.AccidentType>(x => x.IsDeleted == filter.IsDeleted);
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
        static Expression<Func<Entities.Lookups.AccidentType, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Lookups.AccidentType>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.NameAr.Contains(filter.SearchCriteria));
                predicate = predicate.Or(b => b.NameEn.Contains(filter.SearchCriteria));
            }
            return predicate;
        }
        public override async Task<IFinalResult> AddAsync(AddAccidentTypeDto model, CancellationToken cancellationToken = default)
        {
            var IsExisted = await UnitOfWork.Repository.Any(x =>
               (x.NameAr == model.NameAr || x.NameEn == model.NameEn) && !x.IsDeleted, cancellationToken);

            if (IsExisted)
                return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                    message: MessagesConstants.Existed);

            var entity = Mapper.Map<Domain.Entities.Lookups.AccidentType>(model);

            IFinalResult lastEntity = await GetLastRecordAsync(cancellationToken);

            if (lastEntity.Data != null)
            {
                if (lastEntity.Data is AccidentTypeDto accidentTypeDto)
                {
                    if (int.TryParse(accidentTypeDto.Code.AsSpan(accidentTypeDto.Code.Length - 2), out int num))
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

            var result = await UnitOfWork.Repository.AddAsync(entity, cancellationToken);

            var affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);

            if (affectedRows <= 0)
                return ResponseResult.PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                    message: MessagesConstants.AddError);

            return ResponseResult.PostResult(result: entity.Id, status: HttpStatusCode.Created, exception: null,
                message: MessagesConstants.AddSuccess);
        }
        public override async Task<IFinalResult> UpdateAsync(AddAccidentTypeDto model, CancellationToken cancellationToken = default)
        {
            var IsExisted = await UnitOfWork.Repository.Any(x => 
                (x.NameAr == model.NameAr || x.NameEn == model.NameEn) && x.Id != model.Id && !x.IsDeleted, cancellationToken);
   
            if (IsExisted)
                return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                    message: MessagesConstants.Existed);

            Entities.Lookups.AccidentType entityToUpdate = await UnitOfWork.Repository.GetAsync(cancellationToken, model.Id);

            var entity = Mapper.Map(model, entityToUpdate);

            UnitOfWork.Repository.Update(entityToUpdate, entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);

            if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.AddError);

            return ResponseResult.PostResult(result: true, status: HttpStatusCode.OK, exception: null,
                message: MessagesConstants.UpdateSuccess);
        }
    }
}
