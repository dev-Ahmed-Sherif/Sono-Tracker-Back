using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.Governorate.Parameters;
using SonoTracker.Common.DTO.Tracker.Governorate;
using SonoTracker.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Common.Helpers.MediaUploader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SonoTracker.Application.Services.Tracker.Governorate
{
    public class GovernorateService : BaseService<Entities.Tracker.Governorate, AddGovernorateDto, EditGovernorateDto, GovernorateDto, Guid, Guid?>, IGovernorateService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _request;
        private readonly UploaderConfiguration _uploaderConfiguration;

        public GovernorateService(IServiceBaseParameter<Entities.Tracker.Governorate> businessBaseParameter, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor request) : base(businessBaseParameter)
        {
            _hostingEnvironment = hostingEnvironment;
            _request = request;
            _uploaderConfiguration = new UploaderConfiguration(_hostingEnvironment, _request);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.Governorate, bool>> predicate = null)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync();
            var filteredEntities = entity.Where(e => !e.IsDeleted);
            var mapped = Mapper.Map<IEnumerable<Entities.Tracker.Governorate>, IEnumerable<GovernorateDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<GovernorateFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.Governorate>, IEnumerable<GovernorateDto>>(query.Result.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter)
        {

            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.Governorate>, IEnumerable<GovernorateDto>>(query.Result.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }


        static Expression<Func<Entities.Tracker.Governorate, bool>> PredicateBuilderFunction(GovernorateFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.Governorate>(x => x.IsDeleted == filter.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                predicate = predicate.And(x => x.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrWhiteSpace(filter.Url))
            {
                predicate = predicate.And(x => x.Url.Contains(filter.Url));
            }


            return predicate;
        }
        static Expression<Func<Entities.Tracker.Governorate, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.Governorate>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.Name.Contains(filter.SearchCriteria));
                //  predicate = predicate.Or(b => b.Name.Contains(filter.SearchCriteria));
            }
            return predicate;
        }

        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var entitiesToDelete = await UnitOfWork.Repository.FindAsync(d => ids.Contains(d.Id));

            UnitOfWork.Repository.RemoveRange(entitiesToDelete);

            var rows = await UnitOfWork.SaveChangesAsync();

            return ResponseResult.PostResult(result: rows, status: HttpStatusCode.NoContent, message: MessagesConstants.DeleteSuccess);
        }

        public override async Task<IFinalResult> AddAsync(AddGovernorateDto dto)
        {
            var data = await GetAllAsync();

            var dataCollection = data.Data;

            if (dataCollection != null)
                return new ResponseResult().PostResult(result: false, 
                           status: HttpStatusCode.BadRequest, 
                           message: "لا يمكن إضافة أكثر من بيان واحد");

            var mapped = Mapper.Map<Entities.Tracker.Governorate>(dto);

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

        public override async Task<IFinalResult> UpdateAsync([FromForm] AddGovernorateDto dto)
        {
            try
            {
                var entityToUpdate = await UnitOfWork.Repository.GetAsync(dto.Id);

                var newEntity = Mapper.Map(dto, entityToUpdate);

                if (dto.ImageUrl != null)
                {
                    string res = await _uploaderConfiguration.UploadFile(dto.ImageUrl, "Governorate");

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    newEntity.ImageUrl = res;

                    _uploaderConfiguration.DeleteFile(entityToUpdate.ImageUrl);
                }

                SetEntityModifiedBaseProperties(newEntity);
                UnitOfWork.Repository.Update(entityToUpdate, newEntity);
                var affectedRows = await UnitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    Result = ResponseResult.PostResult(result: true, status: HttpStatusCode.Accepted,
                        message: MessagesConstants.UpdateSuccess);
                    return Result;
                }
                else
                {
                    return ResponseResult.PostResult(result: false, status: HttpStatusCode.BadRequest,
                        message: MessagesConstants.UpdateError);
                }
            }
            catch (Exception e)
            {
                //_logger.LogError($"{MessagesConstants.UpdateError}-{nameof(UpdateAsync)}");
                //_logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                throw;
            }
        }

        public override async Task<IFinalResult> DeleteAsync(object id)
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
