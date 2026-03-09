using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.MaintenanceType;
using SonoTracker.Common.DTO.Lookup.MaintenanceType.Parameters;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Lookup.MaintenanceType
{
    public class MaintenanceTypeService(IServiceBaseParameter<Entities.Lookups.MaintenanceType> businessBaseParameter) : BaseService<Entities.Lookups.MaintenanceType, AddMaintenanceTypeDto, EditMaintenanceTypeDto, MaintenanceTypeDto, string, string>(businessBaseParameter), IMaintenanceTypeService
    {
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Lookups.MaintenanceType, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            IEnumerable<Domain.Entities.Lookups.MaintenanceType> entities = await UnitOfWork.Repository.GetAllAsync(disableTracking: disableTracking, cancellationToken: cancellationToken);

            var filtered = entities?.Where(e => !e.IsDeleted) ?? Enumerable.Empty<Domain.Entities.Lookups.MaintenanceType>();
            IEnumerable<MaintenanceTypeDto> mapped = Mapper.Map<IEnumerable<Domain.Entities.Lookups.MaintenanceType>, IEnumerable<MaintenanceTypeDto>>(filtered);

            return ResponseResult.PostResult(result: mapped, status: HttpStatusCode.OK, exception: null,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<MaintenanceTypeFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            (int Count, IEnumerable<Entities.Lookups.MaintenanceType> Result) = await UnitOfWork.Repository.FindPagedAsync(
                predicate: PredicateBuilderFunction(filter.Filter),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                cancellationToken: cancellationToken);

            var filteredResult = Result?.Where(x => x.IsDeleted != true) ?? Enumerable.Empty<Entities.Lookups.MaintenanceType>();
            var data = Mapper.Map<IEnumerable<Entities.Lookups.MaintenanceType>, IEnumerable<MaintenanceTypeDto>>(filteredResult);

            return new PagingResult(filter.PageNumber, filter.PageSize, Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Lookups.MaintenanceType>, IEnumerable<MaintenanceTypeDto>>(query.Result.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        static Expression<Func<Entities.Lookups.MaintenanceType, bool>> PredicateBuilderFunction(MaintenanceTypeFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Lookups.MaintenanceType>(x => x.IsDeleted == filter.IsDeleted);
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
        static Expression<Func<Entities.Lookups.MaintenanceType, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Lookups.MaintenanceType>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.NameAr.Contains(filter.SearchCriteria));
                predicate = predicate.Or(b => b.NameEn.Contains(filter.SearchCriteria));
            }
            return predicate;
        }
        public override async Task<IFinalResult> AddAsync(AddMaintenanceTypeDto model, CancellationToken cancellationToken = default)
        {
            try
            {
                var IsExisted = await UnitOfWork.Repository.Any(x =>
                    (x.NameAr == model.NameAr || x.NameEn == model.NameEn) && !x.IsDeleted, cancellationToken);

                if (IsExisted)
                    return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                        message: MessagesConstants.Existed);

                var entity = Mapper.Map<Entities.Lookups.MaintenanceType>(model);

                IFinalResult lastEntity = await GetLastRecordAsync(cancellationToken);

                if (lastEntity.Data != null)
                {
                    if (lastEntity.Data is MaintenanceTypeDto maintenanceTypeDto)
                    {
                        if (int.TryParse(maintenanceTypeDto.Code.AsSpan(maintenanceTypeDto.Code.Length - 2), out int num))
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
        
        public override async Task<IFinalResult> UpdateAsync(AddMaintenanceTypeDto model, CancellationToken cancellationToken = default)
        {
            var IsExisted = await UnitOfWork.Repository.Any(x =>
                (x.NameAr == model.NameAr || x.NameEn == model.NameEn) && x.Id != model.Id && x.IsDeleted != true, cancellationToken);

            if (IsExisted)
                return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict, exception: null,
                    message: MessagesConstants.Existed);

            Domain.Entities.Lookups.MaintenanceType entityToUpdate = await UnitOfWork.Repository.GetAsync(cancellationToken, model.Id);

            var entity = Mapper.Map(model, entityToUpdate);

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
