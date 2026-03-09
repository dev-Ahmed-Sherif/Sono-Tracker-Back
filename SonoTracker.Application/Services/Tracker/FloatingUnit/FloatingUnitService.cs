using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.FloatingUnit.Parameters;
using SonoTracker.Common.DTO.Tracker.FloatingUnit;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SonoTracker.Common.Helpers.MediaUploader;

namespace SonoTracker.Application.Services.Tracker.FloatingUnit
{
    public class FloatingUnitService : BaseService<Entities.Tracker.FloatingUnit, AddFloatingUnitDto, EditFloatingUnitDto, FloatingUnitDto, string, string>, IFloatingUnitService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _request;
        private readonly UploaderConfiguration _uploaderConfiguration;
        public FloatingUnitService(IServiceBaseParameter<Entities.Tracker.FloatingUnit> businessBaseParameter, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor request) : base(businessBaseParameter)
        {
            _hostingEnvironment = hostingEnvironment;
            _request = request;
            _uploaderConfiguration = new UploaderConfiguration(_hostingEnvironment, _request);
        }
        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id.Equals(id),
                include: src => src
                .Include(t => t.UnitType)
               );
            var mapped = Mapper.Map<Domain.Entities.Tracker.FloatingUnit, EditFloatingUnitDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id.Equals(id),
                include: src => src
                .Include(t => t.UnitType)
               );
            var mapped = Mapper.Map<Domain.Entities.Tracker.FloatingUnit, FloatingUnitDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.FloatingUnit, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
             .Include(t => t.UnitType));
            var filteredEntities = entity.Where(e => !e.IsDeleted);
            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.FloatingUnit>, IEnumerable<FloatingUnitDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<FloatingUnitFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter),
                pageNumber: offset, pageSize: limit,
                filter.OrderByValue,
                include: src => src
               .Include(t => t.UnitType),
                cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.FloatingUnit>, IEnumerable<FloatingUnitDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.FloatingUnit>, IEnumerable<FloatingUnitDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }
        static Expression<Func<Entities.Tracker.FloatingUnit, bool>> PredicateBuilderFunction(FloatingUnitFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.FloatingUnit>(x => x.IsDeleted == filter.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter.NameAr))
            {
                predicate = predicate.And(x => x.NameAr.Contains(filter.NameAr));
            }
            if (!string.IsNullOrWhiteSpace(filter.NameEn))
            {
                predicate = predicate.And(x => x.NameEn.Contains(filter.NameEn));
            }
            if (!string.IsNullOrEmpty(filter.UnitTypeId))
            {
                predicate = predicate.And(x => x.UnitTypeId == filter.UnitTypeId);
            }
            if (!string.IsNullOrWhiteSpace(filter.Code))
            {
                predicate = predicate.And(x => x.Code.Contains(filter.Code));
            }
            if (!string.IsNullOrWhiteSpace(filter.LicenseNumber))
            {
                predicate = predicate.And(x => x.LicenseNumber.Contains(filter.LicenseNumber));
            }
           
            return predicate;
        }
        static Expression<Func<Entities.Tracker.FloatingUnit, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.FloatingUnit>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.NameAr.Contains(filter.SearchCriteria));
                predicate = predicate.Or(b => b.NameEn.Contains(filter.SearchCriteria));
               
            }
            return predicate;
        }
        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var entitiesToDelete = await UnitOfWork.Repository.FindAsync(d => ids.Contains(d.Id), cancellationToken: cancellationToken);

            UnitOfWork.Repository.RemoveRange(entitiesToDelete);

            var rows = await UnitOfWork.SaveChangesAsync(cancellationToken);

            return ResponseResult.PostResult(result: rows, status: HttpStatusCode.NoContent, message: MessagesConstants.DeleteSuccess);
        }
        public override async Task<IFinalResult> AddAsync(AddFloatingUnitDto model, CancellationToken cancellationToken = default)
        {
            var IsExisted = await UnitOfWork.Repository.Any(x =>
            (x.NameAr == model.NameAr
            || x.NameEn == model.NameEn
            || x.Code==model.Code)
            && x.IsDeleted != true);

            if (IsExisted)
                return new ResponseResult().PostResult(result: false, status: HttpStatusCode.BadRequest, message: MessagesConstants.Existed);

            var entity = Mapper.Map<Domain.Entities.Tracker.FloatingUnit>(model);

            var result = await UnitOfWork.Repository.AddAsync(entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync();

            if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.AddError);

            return ResponseResult.PostResult(result: entity.Id, HttpStatusCode.Created, null, MessagesConstants.AddSuccess);
        }
        public override async Task<IFinalResult> UpdateAsync(AddFloatingUnitDto model, CancellationToken cancellationToken = default)
        {
            var IsExisted = await UnitOfWork.Repository.Any(x => (x.NameAr == model.NameAr
            || x.NameEn == model.NameEn
            || x.Code == model.Code)
            && x.IsDeleted != true);

            if (IsExisted)
                return new ResponseResult().PostResult(result: false, status: HttpStatusCode.BadRequest, message: MessagesConstants.Existed);

            Domain.Entities.Tracker.FloatingUnit entityToUpdate = await UnitOfWork.Repository.GetAsync(model.Id);

            var entity = Mapper.Map(model, entityToUpdate);

            UnitOfWork.Repository.Update(entityToUpdate, entity);

            //SetEntityModifiedBaseProperties(entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync();

            if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.AddError);

            return ResponseResult.PostResult(true, HttpStatusCode.OK, null, MessagesConstants.UpdateSuccess);
        }

    }
}
