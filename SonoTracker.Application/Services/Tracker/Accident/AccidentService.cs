using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Common.Helpers.MediaUploader;
using SonoTracker.Common.DTO.Tracker.Accident;
using SonoTracker.Common.DTO.Tracker.Accident.Parameters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SonoTracker.Application.Services.Tracker.FloatingUnit;
using SonoTracker.Common.DTO.Tracker.FloatingUnit;

namespace SonoTracker.Application.Services.Tracker.Accident
{
    public class AccidentService : BaseService<Domain.Entities.Tracker.Accident, AddAccidentDto, EditAccidentDto, AccidentDto, string, string>, IAccidentService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _request;
        private readonly UploaderConfiguration _uploaderConfiguration;
        private readonly IFloatingUnitService _floatingUnitService;
        public AccidentService(IServiceBaseParameter<Entities.Tracker.Accident> businessBaseParameter,
            IWebHostEnvironment hostingEnvironment, IHttpContextAccessor request,
            IFloatingUnitService floatingUnitService) : base(businessBaseParameter)
        {
            _hostingEnvironment = hostingEnvironment;
            _request = request;
            _uploaderConfiguration = new UploaderConfiguration(_hostingEnvironment, _request);
            _floatingUnitService = floatingUnitService;
        }
        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == id.ToString(),
                include: src => src
                .Include(t => t.FloatingUnit)
               .Include(x => x.AccidentType)
               .Include(x => x.Organization)
                );
            var mapped = Mapper.Map<Domain.Entities.Tracker.Accident, EditAccidentDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == id.ToString(),
                               include: src => src
                               .Include(t => t.FloatingUnit)
                               .Include(x => x.AccidentType)
                               .Include(x => x.Organization)
                               .Include(a => a.Town));

            var mapped = Mapper.Map<Domain.Entities.Tracker.Accident, AccidentDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.Accident, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
                                         .Include(t => t.FloatingUnit)
                                         .Include(x => x.AccidentType)
                                         .Include(x => x.Organization)
                                         .Include(a => a.Town));

            var filteredEntities = entity.Where(e => !e.IsDeleted);

            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.Accident>, IEnumerable<AccidentDto>>(filteredEntities);

            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<AccidentFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter),
                    pageNumber: offset,
                    pageSize: limit,
                    filter.OrderByValue,
                    include: src => src
                      .Include(t => t.FloatingUnit)
                      .Include(x => x.AccidentType)
                      .Include(x => x.Organization)
                      .Include(a => a.Town),
                    cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.Accident>, IEnumerable<AccidentDto>>(query.Item2);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.Accident>, IEnumerable<AccidentDto>>(query.Item2);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }
        static Expression<Func<Entities.Tracker.Accident, bool>> PredicateBuilderFunction(AccidentFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.Accident>(x => x.IsDeleted == filter.IsDeleted);

            if (!string.IsNullOrEmpty(filter.TownId))
            {
                predicate = predicate.And(x => x.TownId == filter.TownId);
            }
            if (filter.AccidentDate.HasValue)
            {
                predicate = predicate.And(x => x.AccidentDate == DateOnly.FromDateTime(filter.AccidentDate.Value));
            }
            if (!string.IsNullOrEmpty(filter.OrganizationId))
            {
                predicate = predicate.And(x => x.OrganizationId == filter.OrganizationId);

            }
            if (!string.IsNullOrEmpty(filter.AccidentTypeId))
            {
                predicate = predicate.And(x => x.AccidentTypeId == filter.AccidentTypeId);
            }
            if (!string.IsNullOrEmpty(filter.FloatingUnitId))
            {
                predicate = predicate.And(x => x.FloatingUnitId == filter.FloatingUnitId);
            }
            if (filter.CaseId.HasValue)
            {
                predicate = predicate.And(x => x.Case == filter.CaseId.Value);
            }
            

            return predicate;
        }
        static Expression<Func<Entities.Tracker.Accident, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.Accident>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {

                //  predicate = predicate.Or(b => b.Name.Contains(filter.SearchCriteria));
            }
            return predicate;
        }
        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids)
        {
            var idsList = ids.ToList();
            var entitiesToDelete = await UnitOfWork.Repository.FindAsync(d => idsList.Contains(d.Id));

            UnitOfWork.Repository.RemoveRange(entitiesToDelete);

            var rows = await UnitOfWork.SaveChangesAsync();

            return ResponseResult.PostResult(result: rows, status: HttpStatusCode.NoContent, message: MessagesConstants.DeleteSuccess);
        }
        public override async Task<IFinalResult> AddAsync([FromForm] AddAccidentDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var mapped = Mapper.Map<Entities.Tracker.Accident>(dto);

                var floatingUnit = await _floatingUnitService.GetByIdAsync(dto.FloatingUnitId);

                string floatingUnitCode;

                if (floatingUnit.Data is FloatingUnitDto floatingUnitDto)
                {
                    floatingUnitCode = floatingUnitDto.Code;

                    AccidentFilter filter = new()
                    {
                        FloatingUnitId = dto.FloatingUnitId,
                        IsDeleted = false
                    };

                    // Check if the trip information already exists for the given floating unit id
                    var exist = await GetAllFilterAsync(filter);
                    // Fix: Explicitly cast exist.Data to a collection type to access the Count property.
                    var existDataCollection = exist.Data as ICollection<AccidentDto>;

                    if (existDataCollection?.Count > 0)
                    {
                        // Handle existing trip information logic here (if needed)
                        var lastTrip = existDataCollection.LastOrDefault();
                        if (lastTrip != null && lastTrip.Number.StartsWith(floatingUnitCode))
                        {
                            // Extract the numeric part and increment it
                            var numericPart = lastTrip.Number[floatingUnitCode.Length..];
                            if (int.TryParse(numericPart, out int number))
                            {
                                number++;
                                mapped.Code = floatingUnitCode + number.ToString("D3"); // Ensure 4 digits
                            }
                        }
                    }
                    else
                    {
                        mapped.Code = floatingUnitCode + "001";
                    }
                }

                if (dto.Attach != null)
                {
                    string res = await _uploaderConfiguration.UploadFile(dto.Attach, "Accident");

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    mapped.Attach = res;
                }

                mapped.IsDeleted = false;

                UnitOfWork.Repository.Add(mapped);

                await UnitOfWork.SaveChangesAsync();

                return ResponseResult.PostResult(mapped, status: HttpStatusCode.Created, message: HttpStatusCode.Created.ToString());
            }
            catch (Exception e)
            {
                //_logger.LogError($"{MessagesConstants.AddError}-{nameof(AddAsync)}");
                //_logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                throw;
            }
            
        }
        public override async Task<IFinalResult> UpdateAsync([FromForm] AddAccidentDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityToUpdate = await UnitOfWork.Repository.GetAsync(dto.Id);

                var newEntity = Mapper.Map(dto, entityToUpdate);

                if (entityToUpdate != null)
                {
                    if (dto.Attach != null)
                    {
                        string res = await _uploaderConfiguration.UploadFile(dto.Attach, "Accident");

                        if (res != null)
                        {
                            if (UploadResponse(res) != null)
                                return UploadResponse(res);
                        }

                        newEntity.Attach = res;

                        _uploaderConfiguration.DeleteFile(entityToUpdate.Attach);
                    }
                    if (dto.Attach == null)
                    {
                        var entity = await GetByIdForEditAsync(dto.Id);
                        var entityRes = (EditAccidentDto)entity.Data;
                        newEntity.Attach = entityRes.Attach;
                    }
                }
                else
                {
                    return ResponseResult.PostResult(null, status: HttpStatusCode.NotFound,
                                          message: MessagesConstants.NotFound);
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
                _uploaderConfiguration.DeleteFile(entityToDelete.Attach);

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
        public async Task<IFinalResult> GetAllFilterAsync(AccidentFilter filter)
        {
            var entity = await UnitOfWork.Repository.FindAsync(predicate: PredicateBuilderFunction(filter));

            var data = Mapper.Map<IEnumerable<Entities.Tracker.Accident>, IEnumerable<AccidentDto>>(entity.Where(x => x.IsDeleted != true));

            return ResponseResult.PostResult(data, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
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
