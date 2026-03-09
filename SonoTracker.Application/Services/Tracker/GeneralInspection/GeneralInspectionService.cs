using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.Inspection;
using SonoTracker.Common.Helpers.MediaUploader;
using SonoTracker.Domain;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Common.DTO.Tracker.GeneralInspection;
using SonoTracker.Common.DTO.Tracker.GeneralInspection.Parameters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SonoTracker.Application.Services.Tracker.GeneralInspection
{
    public class GeneralInspectionService : BaseService<Entities.Tracker.Inspection, AddGeneralInspectionDto, EditGeneralInspectionDto, GeneralInspectionDto, string, string>, IGeneralInspectionService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _request;
        private readonly UploaderConfiguration _uploaderConfiguration;
        public GeneralInspectionService(IServiceBaseParameter<Entities.Tracker.Inspection> businessBaseParameter,
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
                .Include(t => t.Organization)
                .Include(t => t.TripInformation)
                .ThenInclude(t => t.FloatingUnit)
                );
            var mapped = Mapper.Map<Entities.Tracker.Inspection, EditGeneralInspectionDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
              include: src => src
                .Include(t => t.Organization)
                .Include(t => t.TripInformation)
                .ThenInclude(t => t.FloatingUnit));
            var mapped = Mapper.Map<Entities.Tracker.Inspection, GeneralInspectionDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Entities.Tracker.Inspection, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync
                (include: src => src
                .Include(t => t.Organization)
                .Include(t => t.TripInformation)
                .ThenInclude(t => t.FloatingUnit)
                );
            var filteredEntities = entity.Where(e => !e.IsDeleted);
            var mapped = Mapper.Map<IEnumerable<Entities.Tracker.Inspection>, IEnumerable<GeneralInspectionDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<GeneralInspectionFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue,
                include: src => src
                .Include(t => t.Organization)
                .Include(t => t.TripInformation)
                .ThenInclude(t => t.FloatingUnit),
                cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.Inspection>, IEnumerable<GeneralInspectionDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }



        static Expression<Func<Entities.Tracker.Inspection, bool>> PredicateBuilderFunction(GeneralInspectionFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.Inspection>(x => x.IsDeleted == filter.IsDeleted);
           
            if (!string.IsNullOrEmpty(filter.OrganizationId))
            {
                predicate = predicate.And(x => x.OrganizationId == filter.OrganizationId);
            }
            if (!string.IsNullOrEmpty(filter.TripInformationId))
            {
                predicate = predicate.And(x => x.TripInformationId == filter.TripInformationId);
            }
            if (filter.InspectionDate.HasValue)
            {
                predicate = predicate.And(x => x.InspectionDate.Date == filter.InspectionDate.Value.Date);
            }
            if (filter.IsInspected)
            {
                predicate = predicate.And(x => x.IsInspected == filter.IsInspected);
            }
            if (!string.IsNullOrEmpty(filter.FloatingUnitId))
            {
                predicate = predicate.And(x => x.TripInformation.FloatingUnitId == filter.FloatingUnitId);
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

        public override async Task<IFinalResult> AddAsync([FromForm] AddGeneralInspectionDto dto, CancellationToken cancellationToken = default)
        {
            var mapped = Mapper.Map<Entities.Tracker.Inspection>(dto);
            if (dto.InspectionAttachment != null)
            {
                string res = await _uploaderConfiguration.UploadFile(dto.InspectionAttachment, "Inspection");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                mapped.InspectionAttachment = res;
            }
            mapped.IsDeleted = false;
            UnitOfWork.Repository.Add(mapped);
            var rows = await UnitOfWork.SaveChangesAsync();
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.Created, message: HttpStatusCode.Created.ToString());
        }

        public override async Task<IFinalResult> UpdateAsync([FromForm] AddGeneralInspectionDto dto, CancellationToken cancellationToken = default)
        {

            try
            {
                
                var entityToUpdate = await UnitOfWork.Repository.GetAsync(dto.Id);
                var newEntity = Mapper.Map(dto, entityToUpdate);
                if (dto.InspectionAttachment != null)
                {
                    string res = await _uploaderConfiguration.UploadFile(dto.InspectionAttachment, "Inspection");

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    newEntity.InspectionAttachment = res;

                    _uploaderConfiguration.DeleteFile(entityToUpdate.InspectionAttachment);
                }

                if (dto.InspectionAttachment == null)
                {
                    var entity = await GetByIdForEditAsync(dto.Id);
                    var entityRes = (EditGeneralInspectionDto)entity.Data;
                    newEntity.InspectionAttachment = entityRes.InspectionAttachment;
                }

                //SetEntityModifiedBaseProperties(newEntity);
                UnitOfWork.Repository.Update(entityToUpdate, newEntity);
                var affectedRows = await UnitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    Result = ResponseResult.PostResult(result: true, status: HttpStatusCode.Accepted,
                        message: MessagesConstants.UpdateSuccess);
                }

                return Result;
            }
            catch (Exception e)
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

                // Reomve Uploaded File
                _uploaderConfiguration.DeleteFile(entityToDelete.InspectionAttachment);

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
