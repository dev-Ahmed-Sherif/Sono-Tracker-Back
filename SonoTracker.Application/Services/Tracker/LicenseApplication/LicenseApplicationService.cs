using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Application.Services.Tracker.FloatingUnit;
using SonoTracker.Application.Services.Tracker.Organization;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.LicenseApplication;
using SonoTracker.Common.DTO.Tracker.LicenseApplication.Parameters;
using SonoTracker.Common.DTO.Tracker.Maintenance.Parameters;
using SonoTracker.Common.DTO.Tracker.Maintenance;
using SonoTracker.Common.Helpers.MediaUploader;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using SonoTracker.Common.DTO.Tracker.FloatingUnit;
using SonoTracker.Domain.Entities.Tracker;
using SonoTracker.Common.DTO.Tracker.Organization;


namespace SonoTracker.Application.Services.Tracker.LicenseApplication
{
    public class LicenseApplicationService : BaseService<Entities.Tracker.LicenseApplication, AddLicenseApplicationDto, EditLicenseApplicationDto, LicenseApplicationDto, string, string>, ILicenseApplicationService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _request;
        private readonly UploaderConfiguration _uploaderConfiguration;
        private readonly IOrganizationService _organizationService;
        public LicenseApplicationService(IServiceBaseParameter<Entities.Tracker.LicenseApplication> businessBaseParameter, 
            IWebHostEnvironment hostingEnvironment, IHttpContextAccessor request) : base(businessBaseParameter)
        {
            _hostingEnvironment = hostingEnvironment;
            _request = request;
            _uploaderConfiguration = new UploaderConfiguration(_hostingEnvironment, _request);
        }
        public override async Task<IFinalResult> GetByIdForEditAsync(object id)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                 include: src => src
                                 .Include(t => t.FromOrganization)
                                 .Include(t => t.ToOrganization));

            var mapped = Mapper.Map<Domain.Entities.Tracker.LicenseApplication, EditLicenseApplicationDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                                .Include(t => t.FromOrganization)
                                .Include(t => t.ToOrganization));

            var mapped = Mapper.Map<Domain.Entities.Tracker.LicenseApplication, LicenseApplicationDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Entities.Tracker.LicenseApplication, bool>> predicate = null)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(
                include: src => src
                .Include(t => t.FromOrganization)
                .Include(t => t.ToOrganization));

            var filteredEntities = entity.Where(e => !e.IsDeleted);

            var mapped = Mapper.Map<IEnumerable<Entities.Tracker.LicenseApplication>, IEnumerable<LicenseApplicationDto>>(filteredEntities);

            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<LicenseApplicationFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.LicenseApplication>, IEnumerable<LicenseApplicationDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.LicenseApplication>, IEnumerable<LicenseApplicationDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }


        static Expression<Func<Entities.Tracker.LicenseApplication, bool>> PredicateBuilderFunction(LicenseApplicationFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.LicenseApplication>(x => x.IsDeleted == filter.IsDeleted);

            if (!string.IsNullOrWhiteSpace(filter.LicenseNumber))
            {
                predicate = predicate.And(x => x.LicenseNumber.Equals(filter.LicenseNumber));
            }
            if (filter.StartDate.HasValue)
            {
                predicate = predicate.And(x => x.LicenseDate > filter.StartDate);
            }
            if (filter.EndDate.HasValue)
            {
                predicate = predicate.And(x => x.LicenseDate < filter.EndDate);
            }
            if (!string.IsNullOrEmpty(filter.FromOrganizationId))
            {
                predicate = predicate.And(x => x.FromOrganizationId == filter.FromOrganizationId);
            }
            if (!string.IsNullOrEmpty(filter.ToOrganizationId))
            {
                predicate = predicate.And(x => x.ToOrganizationId == filter.ToOrganizationId);
            }
            if (filter.Status.HasValue)
            {
                predicate = predicate.And(x => x.Status == filter.Status);
            }
            
            
            return predicate;
        }
        static Expression<Func<Entities.Tracker.LicenseApplication, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.LicenseApplication>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                //predicate = predicate.And(b => b.Name.Contains(filter.SearchCriteria));
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
        public async Task<IFinalResult> GetAllFilterAsync(LicenseApplicationFilter filter)
        {
            var entity = await UnitOfWork.Repository.FindAsync(predicate: PredicateBuilderFunction(filter));

            var data = Mapper.Map<IEnumerable<Entities.Tracker.LicenseApplication>, IEnumerable<LicenseApplicationDto>>(entity.Where(x => x.IsDeleted != true));

            return ResponseResult.PostResult(data, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public override async Task<IFinalResult> AddAsync([FromForm] AddLicenseApplicationDto dto)
        {
            var mapped = Mapper.Map<Entities.Tracker.LicenseApplication>(dto);

            var orgnaization = await _organizationService.GetByIdAsync(dto.FromOrganizationId);

            string organizationCode;

            IFinalResult data = await GetAllAsync();

            if (orgnaization.Data is OrganizationDto orgnaizationDto)
            {
                organizationCode = orgnaizationDto.Code;

                LicenseApplicationFilter filter = new()
                {
                    FromOrganizationId = dto.FromOrganizationId,
                    IsDeleted = false
                };

                // Check if the trip information already exists for the given floating unit id
                var exist = await GetAllFilterAsync(filter);
                // Fix: Explicitly cast exist.Data to a collection type to access the Count property.
                var existDataCollection = exist.Data as ICollection<LicenseApplicationDto>;

                if (existDataCollection?.Count > 0)
                {
                    // Handle existing trip information logic here (if needed)
                    var lastLicenseApplication = existDataCollection.LastOrDefault();
                    if (lastLicenseApplication != null && lastLicenseApplication.LicenseNumber.StartsWith(organizationCode))
                    {
                        // Extract the numeric part and increment it
                        var numericPart = lastLicenseApplication.LicenseNumber[organizationCode.Length..];
                        if (int.TryParse(numericPart, out int number))
                        {
                            number++;
                            mapped.LicenseNumber = organizationCode + number.ToString("D3"); // Ensure 3 digits
                        }
                    }
                }
                else
                {
                    mapped.LicenseNumber = organizationCode + "001";
                }
            }

            if (dto.Insurance != null)
            {
                string res = await _uploaderConfiguration
                                   .UploadFile(dto.Insurance, "LicenseApplication/Insurance");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                mapped.Insurance = res;
            }
            if (dto.CommercialRegister != null)
            {
                string res = await _uploaderConfiguration
                                   .UploadFile(dto.CommercialRegister, "LicenseApplication/CommercialRegister");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                mapped.CommercialRegister = res;
            }
            if (dto.Taxes != null)
            {
                string res = await _uploaderConfiguration
                                   .UploadFile(dto.Taxes, "LicenseApplication/Taxes");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                mapped.Taxes = res;
            }
            if (dto.CivilProtection != null)
            {
                string res = await _uploaderConfiguration
                                   .UploadFile(dto.CivilProtection, "LicenseApplication/CivilProtection");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                mapped.CivilProtection = res;
            }
            if (dto.Irrigation != null)
            {
                string res = await _uploaderConfiguration
                                   .UploadFile(dto.Irrigation, "LicenseApplication/Irragtion");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                mapped.Irrigation = res;
            }
            if (dto.StateProperty != null)
            {
                string res = await _uploaderConfiguration
                                   .UploadFile(dto.StateProperty, "LicenseApplication/StateProperty");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                mapped.StateProperty = res;
            }
            if (dto.Other != null)
            { 
                string res = await _uploaderConfiguration
                                   .UploadFile(dto.Other, "LicenseApplication/Other");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                mapped.Other = res;
            }
            
            mapped.IsDeleted = false;

            UnitOfWork.Repository.Add(mapped);

            await UnitOfWork.SaveChangesAsync();

            return ResponseResult.PostResult(mapped, status: HttpStatusCode.Created, message: HttpStatusCode.Created.ToString());
        }

        public override async Task<IFinalResult> UpdateAsync([FromForm] AddLicenseApplicationDto dto)
        {
            try
            {
                var entityToUpdate = await UnitOfWork.Repository.GetAsync(dto.Id);
                
                var newEntity = Mapper.Map(dto, entityToUpdate);

                if (dto.Insurance != null)
                {
                    string res = await _uploaderConfiguration
                                   .UploadFile(dto.Insurance, "LicenseApplication/Insurance");

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    newEntity.Insurance = res;

                    _uploaderConfiguration.DeleteFile(entityToUpdate.Insurance);
                }
                else
                {
                    var entity = await GetByIdAsync(dto.Id);
                    var entityRes = (LicenseApplicationDto)entity.Data;
                    newEntity.Insurance = entityRes.Insurance;
                }
                
                if (dto.CommercialRegister != null)
                {
                    string res = await _uploaderConfiguration
                                   .UploadFile(dto.CommercialRegister, "LicenseApplication/CommercialRegister");

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    newEntity.CommercialRegister = res;

                    _uploaderConfiguration.DeleteFile(entityToUpdate.CommercialRegister);
                }
                else
                {
                    var entity = await GetByIdAsync(dto.Id);
                    var entityRes = (LicenseApplicationDto)entity.Data;
                    newEntity.CommercialRegister = entityRes.CommercialRegister;
                }

                if (dto.Taxes != null)
                {
                    string res = await _uploaderConfiguration
                                   .UploadFile(dto.Taxes, "LicenseApplication/Taxes");

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    newEntity.Taxes = res;

                    _uploaderConfiguration.DeleteFile(entityToUpdate.Taxes);
                }
                else
                {
                    var entity = await GetByIdAsync(dto.Id);
                    var entityRes = (LicenseApplicationDto)entity.Data;
                    newEntity.Taxes = entityRes.Taxes;
                }

                if (dto.CivilProtection != null)
                {
                    string res = await _uploaderConfiguration
                                    .UploadFile(dto.CommercialRegister, "LicenseApplication/CivilProtection");

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    newEntity.CivilProtection = res;

                    _uploaderConfiguration.DeleteFile(entityToUpdate.CivilProtection);
                }
                else
                {
                    var entity = await GetByIdAsync(dto.Id);
                    var entityRes = (LicenseApplicationDto)entity.Data;
                    newEntity.CivilProtection = entityRes.CivilProtection;
                }

                if (dto.Irrigation != null)
                {
                    string res = await _uploaderConfiguration
                                 .UploadFile(dto.Irrigation, "LicenseApplication/Irrigation");

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    newEntity.Irrigation = res;

                    _uploaderConfiguration.DeleteFile(entityToUpdate.Irrigation);
                }
                else
                {
                    var entity = await GetByIdAsync(dto.Id);
                    var entityRes = (LicenseApplicationDto)entity.Data;
                    newEntity.Irrigation = entityRes.Irrigation;
                }

                if (dto.StateProperty != null)
                {
                    string res = await _uploaderConfiguration
                                   .UploadFile(dto.StateProperty, "LicenseApplication/StateProperty");

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    newEntity.StateProperty = res;

                    _uploaderConfiguration.DeleteFile(entityToUpdate.StateProperty);
                }
                else
                {
                    var entity = await GetByIdAsync(dto.Id);
                    var entityRes = (LicenseApplicationDto)entity.Data;
                    newEntity.StateProperty = entityRes.StateProperty;
                }

                if (dto.Other != null)
                {
                    string res = await _uploaderConfiguration
                                 .UploadFile(dto.CommercialRegister, "LicenseApplication/Other");

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    newEntity.Other = res;

                    _uploaderConfiguration.DeleteFile(entityToUpdate.Other);
                }
                else
                {
                    var entity = await GetByIdAsync(dto.Id);
                    var entityRes = (LicenseApplicationDto)entity.Data;
                    newEntity.Other = entityRes.Other;
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

        public override async Task<IFinalResult> DeleteAsync(object id)
        {
            try
            {
                var entityToDelete = await UnitOfWork.Repository.GetAsync(id);

                // Remove Uploaded Files
                _uploaderConfiguration.DeleteFile(entityToDelete.Insurance);
                _uploaderConfiguration.DeleteFile(entityToDelete.CommercialRegister);
                _uploaderConfiguration.DeleteFile(entityToDelete.Taxes);
                _uploaderConfiguration.DeleteFile(entityToDelete.CivilProtection);
                _uploaderConfiguration.DeleteFile(entityToDelete.Irrigation);
                _uploaderConfiguration.DeleteFile(entityToDelete.StateProperty);
                _uploaderConfiguration.DeleteFile(entityToDelete.Other);

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
