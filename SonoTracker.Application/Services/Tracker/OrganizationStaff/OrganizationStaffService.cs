using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Application.Services.Tracker.Organization;
using SonoTracker.Application.Services.Tracker.OrganizationStaffStaff;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.Organization;
using SonoTracker.Common.DTO.Tracker.OrganizationStaff;
using SonoTracker.Common.DTO.Tracker.OrganizationStaff.Parameters;
using SonoTracker.Common.Helpers.MediaUploader;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.OrganizationStaff
{
    public class OrganizationStaffService : BaseService<Entities.Tracker.OrganizationStaff, AddOrganizationStaffDto, EditOrganizationStaffDto, OrganizationStaffDto, string, string>, IOrganizationStaffService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _request;
        private readonly UploaderConfiguration _uploaderConfiguration;

        private readonly IOrganizationService _organizationService;
        public OrganizationStaffService(
            IServiceBaseParameter<Entities.Tracker.OrganizationStaff> businessBaseParameter,
            IWebHostEnvironment hostingEnvironment, IHttpContextAccessor request,
            IOrganizationService organizationService) : base(businessBaseParameter)
        {
            _hostingEnvironment = hostingEnvironment;
            _request = request;
            _uploaderConfiguration = new UploaderConfiguration(_hostingEnvironment, _request);
            _organizationService = organizationService;
        }
        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                .Include(t => t.Nationality)
               .Include(x => x.Organization)
                );
            var mapped = Mapper.Map<Domain.Entities.Tracker.OrganizationStaff, EditOrganizationStaffDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                         .Include(t => t.Organization)
                         .Include(x => x.Nationality));

            var mapped = Mapper.Map<Domain.Entities.Tracker.OrganizationStaff, OrganizationStaffDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.OrganizationStaff, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
             .Include(t => t.Nationality)
             .Include(x => x.Organization));
            var filteredEntities = entity.Where(e => !e.IsDeleted);
            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.OrganizationStaff>, IEnumerable<OrganizationStaffDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<OrganizationStaffFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue,
                include: src => src
             .Include(t => t.Nationality)
             .Include(x => x.Organization),
                cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.OrganizationStaff>, IEnumerable<OrganizationStaffDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.OrganizationStaff>, IEnumerable<OrganizationStaffDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }


        static Expression<Func<Entities.Tracker.OrganizationStaff, bool>> PredicateBuilderFunction(OrganizationStaffFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.OrganizationStaff>(x => x.IsDeleted != true );
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                predicate = predicate.And(x => x.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrWhiteSpace(filter.Job))
            {
                predicate = predicate.And(x => x.Job.Contains(filter.Job));
            }
            if (!string.IsNullOrEmpty(filter.NationalityId))
            {
                predicate = predicate.And(x => x.NationalityId == filter.NationalityId);
            }
            if (!string.IsNullOrEmpty(filter.OrganizationId))
            {
                predicate = predicate.And(x => x.OrganizationId == filter.OrganizationId);
            }
            if (filter.IDType.HasValue)
            {
                predicate = predicate.And(x => x.IDType== filter.IDType.Value);
            }
            if (filter.Gender.HasValue)
            {
                predicate = predicate.And(x => x.Gender == filter.Gender.Value);
            }
            if (filter.IsDelegate)
            {
                predicate = predicate.And(x => x.IsDelegate == filter.IsDelegate);
            }

            return predicate;
        }
        static Expression<Func<Entities.Tracker.OrganizationStaff, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.OrganizationStaff>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.Name.Contains(filter.SearchCriteria));
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
        public override async Task<IFinalResult> AddAsync(AddOrganizationStaffDto model, CancellationToken cancellationToken = default)
        {
            var entity = Mapper.Map<Entities.Tracker.OrganizationStaff>(model);

            if (model.DelegateAttachment != null)
            {
                string res = await _uploaderConfiguration
                                   .UploadFile(model.DelegateAttachment, "OrganizationStaff");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                entity.DelegateAttachment = res;

                entity.IsDelegate = true;
            }
            else
            {
                entity.IsDelegate = false;
            }

            var org = await _organizationService.GetByIdAsync(model.OrganizationId);
            
            if (org != null) 
            {
                var entityRes = (OrganizationDto)org.Data;
                entity.Phone = entityRes.Phone;   
            }
            else
            {
                return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null,message: "Org Id Not Correct");
            }

            await UnitOfWork.Repository.AddAsync(entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync();

            if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.AddError);

            return ResponseResult.PostResult(result: entity.Id, HttpStatusCode.Created, null, MessagesConstants.AddSuccess);
        }
        public override async Task<IFinalResult> UpdateAsync(AddOrganizationStaffDto model, CancellationToken cancellationToken = default)
        {
            Domain.Entities.Tracker.OrganizationStaff entityToUpdate = await UnitOfWork.Repository.GetAsync(model.Id);

            var entity = Mapper.Map(model, entityToUpdate);

            if (model.DelegateAttachment != null)
            {
                string res = await _uploaderConfiguration
                               .UploadFile(model.DelegateAttachment, "OrganizationStaff");

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
                var entityRes = (EditOrganizationStaffDto)entityExist.Data;
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
