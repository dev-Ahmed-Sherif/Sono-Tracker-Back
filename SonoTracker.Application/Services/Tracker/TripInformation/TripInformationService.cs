using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripInformation;
using SonoTracker.Common.DTO.Tracker.TripInformation.Parameters;
using SonoTracker.Common.Helpers.MediaUploader;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.TripInformation
{
    public class TripInformationService : BaseService<Entities.Tracker.TripInformation, AddTripInformationDto, EditTripInformationDto, TripInformationDto, string, string>, ITripInformationService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _request;
        private readonly UploaderConfiguration _uploaderConfiguration;
        public TripInformationService(IServiceBaseParameter<Entities.Tracker.TripInformation> businessBaseParameter, 
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
                .Include(t => t.FloatingUnit)
               .Include(x => x.Route)
                );
            var mapped = Mapper.Map<Domain.Entities.Tracker.TripInformation, EditTripInformationDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                .Include(t => t.FloatingUnit)
               .Include(x => x.Route));
            var mapped = Mapper.Map<Domain.Entities.Tracker.TripInformation, TripInformationDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.TripInformation, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
                                                    .Include(t => t.FloatingUnit)
                                                    .Include(x => x.Route));

            var filteredEntities = entity.Where(e => !e.IsDeleted);

            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.TripInformation>, IEnumerable<TripInformationDto>>(filteredEntities);

            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<IFinalResult> GetAllFilterAsync(TripInformationFilter filter, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.FindAsync(predicate: PredicateBuilderFunction(filter), cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripInformation>, IEnumerable<TripInformationDto>>(entity.Where(x => x.IsDeleted != true));

            return ResponseResult.PostResult(data, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<TripInformationFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter),
                pageNumber: offset, pageSize: limit,
                filter.OrderByValue,
                include: src => src
                .Include(t => t.FloatingUnit)
                .Include(x => x.Route),
                cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripInformation>, IEnumerable<TripInformationDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripInformation>, IEnumerable<TripInformationDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }
        static Expression<Func<Entities.Tracker.TripInformation, bool>> PredicateBuilderFunction(TripInformationFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TripInformation>(x => x.IsDeleted == filter.IsDeleted);

            if (!string.IsNullOrEmpty(filter.FloatingUnitId))
            {
                predicate = predicate.And(x => x.FloatingUnitId == filter.FloatingUnitId);
            }
            if (!string.IsNullOrEmpty(filter.RouteId))
            {
                predicate = predicate.And(x => x.RouteId == filter.RouteId);
            }
            if (filter.SartDate.HasValue)
            {
                predicate = predicate.And(x => x.SartDate.Date >= filter.SartDate.Value.Date);
            }
            if (filter.EndDate.HasValue)
            {
                predicate = predicate.And(x => x.EndDate<= filter.EndDate.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(filter.Code))
            {
                predicate = predicate.And(x => x.Code.Contains(filter.Code));
            }

            return predicate;
        }
        static Expression<Func<Entities.Tracker.TripInformation, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TripInformation>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.Code.Contains(filter.SearchCriteria));
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
        public override async Task<IFinalResult> AddAsync([FromForm] AddTripInformationDto dto, CancellationToken cancellationToken = default)
        {
            var mapped = Mapper.Map<Domain.Entities.Tracker.TripInformation>(dto);

            if (dto.PassengerAttachment != null)
            {
                string res = await _uploaderConfiguration.UploadFile(dto.PassengerAttachment, "TripInformation");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                mapped.PassengerAttachment = res;
            }

            mapped.IsDeleted = false;

            UnitOfWork.Repository.Add(mapped);

            var rows = await UnitOfWork.SaveChangesAsync();

            return ResponseResult.PostResult(mapped, status: HttpStatusCode.Created, message: HttpStatusCode.Created.ToString());
        }
        public override async Task<IFinalResult> UpdateAsync([FromForm] AddTripInformationDto dto, CancellationToken cancellationToken = default)
        {

            try
            {
                
                var entityToUpdate = await UnitOfWork.Repository.GetAsync(dto.Id);

                var newEntity = Mapper.Map(dto, entityToUpdate);

                if (entityToUpdate != null) 
                {
                    if (dto.PassengerAttachment != null)
                    {
                        string res = await _uploaderConfiguration.UploadFile(dto.PassengerAttachment, "TripInformation");

                        if (res != null)
                        {
                            if (UploadResponse(res) != null)
                                return UploadResponse(res);
                        }

                        newEntity.PassengerAttachment = res;

                        _uploaderConfiguration.DeleteFile(entityToUpdate.PassengerAttachment);
                    }
                }

                if (dto.PassengerAttachment == null)
                {
                    var entity = await GetByIdForEditAsync(dto.Id);
                    var entityRes = (EditTripInformationDto)entity.Data;
                    newEntity.PassengerAttachment = entityRes.PassengerAttachment;
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
                _uploaderConfiguration.DeleteFile(entityToDelete.PassengerAttachment);

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
