using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.FloatingUnit;
using SonoTracker.Common.DTO.Tracker.FloatingUnitStaff;
using SonoTracker.Common.DTO.Tracker.FloatingUnitStaff.Parameters;
using SonoTracker.Common.DTO.Tracker.OrganizationStaff;
using SonoTracker.Common.Helpers.MediaUploader;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.FloatingUnitStaff
{
    public class FloatingUnitStaffService : BaseService<Entities.Tracker.FloatingUnitStaff, AddFloatingUnitStaffDto, EditFloatingUnitStaffDto, FloatingUnitStaffDto, string, string>, IFloatingUnitStaffService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _request;
        private readonly UploaderConfiguration _uploaderConfiguration;
        public FloatingUnitStaffService(IServiceBaseParameter<Entities.Tracker.FloatingUnitStaff> businessBaseParameter,
            IWebHostEnvironment hostingEnvironment, IHttpContextAccessor request) : base(businessBaseParameter)
        {
            _hostingEnvironment = hostingEnvironment;
            _request = request;
            _uploaderConfiguration = new UploaderConfiguration(_hostingEnvironment, _request);
        }


        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
            
                .Include(t => t.Nationality));
            var mapped = Mapper.Map<Domain.Entities.Tracker.FloatingUnitStaff, EditFloatingUnitStaffDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                .Include(t => t.FloatingUnit)
                .Include(t => t.Nationality));
            var mapped = Mapper.Map<Domain.Entities.Tracker.FloatingUnitStaff, EditFloatingUnitStaffDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.FloatingUnitStaff, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
             .Include(t => t.FloatingUnit)
             .Include(t => t.Nationality));
            var governorateId = IsSuperAdmin() ? null : GetGovernorateIdFromClaims();
            var filteredEntities = IsSuperAdmin()
                ? entity
                : entity.Where(e => !e.IsDeleted && (string.IsNullOrWhiteSpace(governorateId) || e.GovernorateId == governorateId));
            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.FloatingUnitStaff>, IEnumerable<FloatingUnitStaffDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<FloatingUnitStaffFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var floatingUnitStaffFilter = filter?.Filter ?? new FloatingUnitStaffFilter();
            var governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();

            if (!isSuperAdmin)
                floatingUnitStaffFilter.IsDeleted = false;

            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(floatingUnitStaffFilter, governorateId), pageNumber: offset, pageSize: limit, filter.OrderByValue, cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.FloatingUnitStaff>, IEnumerable<FloatingUnitStaffDto>>(query.Item2);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
     
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var isSuperAdmin = IsSuperAdmin();
            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, cancellationToken: cancellationToken);

            var items = isSuperAdmin ? query.Item2 : query.Item2.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Tracker.FloatingUnitStaff>, IEnumerable<FloatingUnitStaffDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }

        static Expression<Func<Entities.Tracker.FloatingUnitStaff, bool>> PredicateBuilderFunction(FloatingUnitStaffFilter filter, string governorateId = null)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.FloatingUnitStaff>(x => x.IsDeleted == filter.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                predicate = predicate.And(x => x.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrWhiteSpace(filter.Mobile))
            {
                predicate = predicate.And(x => x.Mobile.Contains(filter.Mobile));
            }
            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                predicate = predicate.And(x => x.Email.Contains(filter.Email));
            }
            if (!string.IsNullOrWhiteSpace(filter.Identity))
            {
                predicate = predicate.And(x => x.Identity.Contains(filter.Identity));
            }
            if (filter.gender.HasValue)
            {
                predicate = predicate.And(x => x.Gender == filter.gender.Value);
            }
            if (filter.IDType.HasValue)
            {
                predicate = predicate.And(x => x.IDType == filter.IDType.Value);
            }
            if (!string.IsNullOrEmpty(filter.FloatingUnitId))
            {
                predicate = predicate.And(x => x.FloatingUnitId == filter.FloatingUnitId);
            }
            if (!string.IsNullOrEmpty(filter.NationalityId))
            {
                predicate = predicate.And(x => x.NationalityId == filter.NationalityId);
            }
            if (filter.IsDelegate.HasValue)
            {
                predicate = predicate.And(x => x.IsDelegate == filter.IsDelegate.Value);
            }

            if (!string.IsNullOrWhiteSpace(governorateId))
            {
                predicate = predicate.And(x => x.GovernorateId == governorateId);
            }

            return predicate;
        }
       
        static Expression<Func<Entities.Tracker.FloatingUnitStaff, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.FloatingUnitStaff>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.Name.Contains(filter.SearchCriteria));
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

        public override async Task<IFinalResult> AddAsync(AddFloatingUnitStaffDto model, CancellationToken cancellationToken = default)
        {
            var entity = Mapper.Map<Domain.Entities.Tracker.FloatingUnitStaff>(model);

            if (model.DelegateAttachment != null)
            {
                string res = await _uploaderConfiguration
                                   .UploadFile(model.DelegateAttachment, "FloatingUnitStaff");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                entity.DelegateAttachment = res;

                entity.IsDelegate = true;
            }

            entity.IsDeleted = false;

            await UnitOfWork.Repository.AddAsync(entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync();

            if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.AddError);

            return ResponseResult.PostResult(result: entity.Id, HttpStatusCode.Created, null, MessagesConstants.AddSuccess);
        }
        public override async Task<IFinalResult> UpdateAsync(AddFloatingUnitStaffDto model, CancellationToken cancellationToken = default)
        {
            Domain.Entities.Tracker.FloatingUnitStaff entityToUpdate = await UnitOfWork.Repository.GetAsync(model.Id);

            var entity = Mapper.Map(model, entityToUpdate);

            if (model.DelegateAttachment != null)
            {
                string res = await _uploaderConfiguration
                               .UploadFile(model.DelegateAttachment, "FloatingUnitStaff");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                entity.DelegateAttachment = res;

                entity.IsDelegate = true;

                _uploaderConfiguration.DeleteFile(entityToUpdate.DelegateAttachment);
            }
            else if (model.IsDelegate == false)
            {
                _uploaderConfiguration.DeleteFile(entityToUpdate.DelegateAttachment);

                entity.DelegateAttachment = "";
            }
            else
            {
                var entityExist = await GetByIdForEditAsync(model.Id);
                var entityRes = (EditFloatingUnitStaffDto)entityExist.Data;
                entity.DelegateAttachment = entityRes.DelegateAttachment;
            }

            UnitOfWork.Repository.Update(entityToUpdate, entity);

            //SetEntityModifiedBaseProperties(entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync();

            if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.AddError);

            return ResponseResult.PostResult(true, HttpStatusCode.OK, null, MessagesConstants.UpdateSuccess);
        }
        public override async Task<IFinalResult> DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityToDelete = await UnitOfWork.Repository.GetAsync(id);

                // Remove Uploaded File
                _uploaderConfiguration.DeleteFile(entityToDelete.DelegateAttachment);

                UnitOfWork.Repository.Remove(entityToDelete);
                var affectedRows = await UnitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    Result = ResponseResult.PostResult(result: true, status: HttpStatusCode.Accepted,
                        message: MessagesConstants.DeleteSuccess);
                }

                return Result;
            }
            catch (Exception e)
            {
                //_logger.LogError($"{MessagesConstants.DeleteError}-{nameof(DeleteAsync)}");
                //_logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                throw;
            }

        }
        private IFinalResult UploadResponse(string res)
        {
            if (res == "Size")
            {
                var message = "File Size Larger than 5 Mega Bytes";
                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, message: message);
            }
            else if (res == "Type")
            {
                var message = "File type not allowed.";
                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, message: message);
            }
            else
            {
                return null;
            }
        }
    }
}
