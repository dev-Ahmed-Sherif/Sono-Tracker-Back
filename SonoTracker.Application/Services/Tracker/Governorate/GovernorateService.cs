using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Constants.Auth;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.Governorate;
using SonoTracker.Common.DTO.Tracker.Governorate.Parameters;
using SonoTracker.Common.DTO.Tracker.Organization;
using SonoTracker.Common.Helpers.MediaUploader;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.Governorate
{
    public class GovernorateService : BaseService<Entities.Lookups.Governorate, AddGovernorateDto, EditGovernorateDto, GovernorateDto, string, string>, IGovernorateService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _request;
        private readonly UploaderConfiguration _uploaderConfiguration;

        public GovernorateService(IServiceBaseParameter<Entities.Lookups.Governorate> businessBaseParameter, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor request) : base(businessBaseParameter)
        {
            _hostingEnvironment = hostingEnvironment;
            _request = request;
            _uploaderConfiguration = new UploaderConfiguration(_hostingEnvironment, _request);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Entities.Lookups.Governorate, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(cancellationToken: cancellationToken);
            var governorateId = IsSuperAdmin() ? null : GetGovernorateIdFromClaims();
            var filteredEntities = IsSuperAdmin()
                ? entity
                : entity.Where(e => !e.IsDeleted && (string.IsNullOrWhiteSpace(governorateId) || e.Id == governorateId));
            var mapped = Mapper.Map<IEnumerable<Entities.Lookups.Governorate>, IEnumerable<GovernorateDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<GovernorateFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var governorateFilter = filter?.Filter ?? new GovernorateFilter();
            if (!isSuperAdmin)
                governorateFilter.IsDeleted = false;
            var governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();

            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(
                predicate: PredicateBuilderFunction(governorateFilter, governorateId),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Lookups.Governorate>, IEnumerable<GovernorateDto>>(query.Result);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var isSuperAdmin = IsSuperAdmin();
            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, cancellationToken: cancellationToken);

            var items = isSuperAdmin ? query.Result : query.Result.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Lookups.Governorate>, IEnumerable<GovernorateDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }


        static Expression<Func<Entities.Lookups.Governorate, bool>> PredicateBuilderFunction(GovernorateFilter filter, string governorateId)
        {
            var predicate = PredicateBuilder.New<Entities.Lookups.Governorate>(x => x.IsDeleted == filter.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                predicate = predicate.And(x => x.NameAr.Contains(filter.Name));
            }
            if (!string.IsNullOrWhiteSpace(filter.Url))
            {
                predicate = predicate.And(x => x.WebsiteUrl.Contains(filter.Url));
            }
            if (!string.IsNullOrWhiteSpace(governorateId))
            {
                predicate = predicate.And(x => x.Id == governorateId);
            }


            return predicate;
        }
        static Expression<Func<Entities.Lookups.Governorate, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Lookups.Governorate>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.NameAr.Contains(filter.SearchCriteria));
                //  predicate = predicate.Or(b => b.Name.Contains(filter.SearchCriteria));
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

        public override async Task<IFinalResult> AddAsync(AddGovernorateDto dto, CancellationToken cancellationToken = default)
        {
            var data = await GetAllAsync();

            var dataCollection = data.Data;

            if (dataCollection != null)
                return new ResponseResult().PostResult(result: false, 
                           status: HttpStatusCode.BadRequest, 
                           message: "?? ???? ????? ???? ?? ???? ????");

            var mapped = Mapper.Map<Entities.Lookups.Governorate>(dto);

            if (dto.ImageUrl != null)
            {
                string res = await _uploaderConfiguration.UploadFile(dto.ImageUrl, "Governorate");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                mapped.ImageUrl = res;
            }

            mapped.IsDeleted = false;

            UnitOfWork.Repository.Add(mapped);

            await UnitOfWork.SaveChangesAsync();

            return ResponseResult.PostResult(mapped, status: HttpStatusCode.Created, message: HttpStatusCode.Created.ToString());
        }

        public override async Task<IFinalResult> UpdateAsync([FromForm] AddGovernorateDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityToUpdate = await UnitOfWork.Repository.GetAsync(dto.Id);

                var currentImageUrl = entityToUpdate.ImageUrl;

                var newEntity = Mapper.Map(dto, entityToUpdate);
                
                if (IsSuperAdmin())
                {
                    if (entityToUpdate.IsDeleted)
                        newEntity.IsDeleted = false;
                }

                if (dto.ImageUrl != null)
                {
                    string res = await _uploaderConfiguration.UploadFile(dto.ImageUrl, "Governorate", cancellationToken);

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    _uploaderConfiguration.DeleteFile(currentImageUrl);

                    newEntity.ImageUrl = res;
                }
                else
                {
                    // Keep existing image when no new file is uploaded.
                    newEntity.ImageUrl = currentImageUrl;
                }

                //SetEntityModifiedBaseProperties(newEntity);
                UnitOfWork.Repository.Update(entityToUpdate, newEntity);

                var affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);
                if (affectedRows < 0)
                    return ResponseResult.PostResult(result: false, status: HttpStatusCode.BadRequest,
                        message: MessagesConstants.UpdateError);
                    
                
                return ResponseResult.PostResult(result: true, status: HttpStatusCode.Accepted,
                        message: MessagesConstants.UpdateSuccess);
            }
            catch (Exception)
            {
                //_logger.LogError($"{MessagesConstants.UpdateError}-{nameof(UpdateAsync)}");
                //_logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                throw;
            }
        }

        public override async Task<IFinalResult> DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityToDelete = await UnitOfWork.Repository.GetAsync(id);

                if (entityToDelete == null)
                {
                    return ResponseResult.PostResult(result: false, status: HttpStatusCode.BadRequest,
                        message: MessagesConstants.DeleteError);
                }
                // Reomve Uploaded File
                _uploaderConfiguration.DeleteFile(entityToDelete.ImageUrl);

                UnitOfWork.Repository.Remove(entityToDelete);
                var affectedRows = await UnitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    Result = ResponseResult.PostResult(result: true, status: HttpStatusCode.Accepted,
                        message: MessagesConstants.DeleteSuccess);
                }

                return Result;
            }
            catch (Exception)
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
