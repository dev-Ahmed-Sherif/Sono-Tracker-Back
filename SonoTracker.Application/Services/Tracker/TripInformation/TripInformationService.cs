using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Application.Services.LookUp.Attachments;
using SonoTracker.Application.Services.Tracker.FloatingUnits;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.Attach;
using SonoTracker.Common.DTO.Tracker.FloatingUnit;
using SonoTracker.Common.DTO.Tracker.TripInformation;
using SonoTracker.Common.DTO.Tracker.TripInformation.Parameters;
using SonoTracker.Common.Helpers;
using SonoTracker.Common.Helpers.MediaUploader;
using SonoTracker.Domain;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IFloatingUnitService _floatingUnitService;
        private readonly IAttachmentService _attachService;

        public TripInformationService(IServiceBaseParameter<Entities.Tracker.TripInformation> businessBaseParameter,
            IWebHostEnvironment hostingEnvironment, IHttpContextAccessor request,
            IFloatingUnitService floatingUnitService, IAttachmentService attachService) : base(businessBaseParameter)
        {
            _hostingEnvironment = hostingEnvironment;
            _request = request;
            _uploaderConfiguration = new UploaderConfiguration(_hostingEnvironment, _request);
            _floatingUnitService = floatingUnitService;
            _attachService = attachService;
        }
        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                .Include(t => t.FloatingUnit)
                .Include(x => x.Route)
                .Include(x => x.Governorate)
                .Include(x => x.TripAttachments).ThenInclude(a => a.Attachment), cancellationToken: cancellationToken);
            var mapped = Mapper.Map<Domain.Entities.Tracker.TripInformation, EditTripInformationDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                .Include(t => t.FloatingUnit)
                .Include(x => x.Route)
                .Include(x => x.Governorate)
                .Include(x => x.TripAttachments).ThenInclude(a => a.Attachment), cancellationToken: cancellationToken);
            var mapped = Mapper.Map<Domain.Entities.Tracker.TripInformation, TripInformationDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.TripInformation, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
                                                    .Include(t => t.FloatingUnit)
                                                    .Include(x => x.Route)
                                                    .Include(x => x.Governorate)
                                                    .Include(x => x.TripAttachments).ThenInclude(a => a.Attachment), cancellationToken: cancellationToken);

            var governorateId = IsSuperAdmin() ? null : GetGovernorateIdFromClaims();
            var filteredEntities = IsSuperAdmin()
                ? entity
                : entity.Where(e => !e.IsDeleted && (string.IsNullOrWhiteSpace(governorateId) || e.GovernorateId == governorateId));

            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.TripInformation>, IEnumerable<TripInformationDto>>(filteredEntities);

            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<IFinalResult> GetAllFilterAsync(TripInformationFilter filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var tripFilter = filter ?? new TripInformationFilter();
            var governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();
            if (!isSuperAdmin)
                tripFilter.IsDeleted = false;

            var entity = await UnitOfWork.Repository.FindAsync(
                predicate: PredicateBuilderFunction(tripFilter, governorateId),
                include: src => src
                    .Include(t => t.FloatingUnit)
                    .Include(x => x.Route)
                    .Include(x => x.Governorate)
                    .Include(x => x.TripAttachments).ThenInclude(a => a.Attachment),
                cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripInformation>, IEnumerable<TripInformationDto>>(entity);

            return ResponseResult.PostResult(data, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<TripInformationFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var tripFilter = filter?.Filter ?? new TripInformationFilter();
            var governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();
            if (!isSuperAdmin)
                tripFilter.IsDeleted = false;

            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(tripFilter, governorateId),
                pageNumber: offset, pageSize: limit,
                filter.OrderByValue,
                include: src => src
                .Include(t => t.FloatingUnit)
                .Include(x => x.Route)
                .Include(x => x.Governorate)
                .Include(x => x.TripAttachments).ThenInclude(a => a.Attachment),
                cancellationToken: cancellationToken);

            var items = isSuperAdmin ? query.Item2 : query.Item2.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripInformation>, IEnumerable<TripInformationDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var governorateId = isSuperAdmin ? null : GetGovernorateIdFromClaims();
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter, governorateId);

            var query = await UnitOfWork.Repository.FindPagedAsync(
                predicate: predicate,
                pageNumber: offset,
                pageSize: limit,
                include: src => src
                    .Include(t => t.FloatingUnit)
                    .Include(x => x.Route)
                    .Include(x => x.Governorate),
                cancellationToken: cancellationToken);

            var items = isSuperAdmin ? query.Item2 : query.Item2.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripInformation>, IEnumerable<TripInformationDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }
        static Expression<Func<Entities.Tracker.TripInformation, bool>> PredicateBuilderFunction(TripInformationFilter filter, string governorateId = null)
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
            if (!string.IsNullOrEmpty(filter.GovernorateId))
            {
                predicate = predicate.And(x => x.GovernorateId == filter.GovernorateId);
            }
            if (filter.StartDate.HasValue)
            {
                predicate = predicate.And(x => x.StartDate >= filter.StartDate.Value);
            }
            if (filter.EndDate.HasValue)
            {
                predicate = predicate.And(x => x.EndDate <= filter.EndDate);
            }
            if (!string.IsNullOrWhiteSpace(filter.Code))
            {
                predicate = predicate.And(x => x.Code.Contains(filter.Code));
            }

            if (!string.IsNullOrWhiteSpace(governorateId))
            {
                predicate = predicate.And(x => x.GovernorateId == governorateId);
            }

            return predicate;
        }
        static Expression<Func<Entities.Tracker.TripInformation, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter, string governorateId = null)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TripInformation>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.Code.Contains(filter.SearchCriteria));
            }
            if (!string.IsNullOrWhiteSpace(governorateId))
            {
                predicate = predicate.And(x => x.GovernorateId == governorateId);
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
        public override async Task<IFinalResult> AddAsync(AddTripInformationDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var nationalityRepo = UnitOfWork.GetRepository<Nationality>();
                var nationalities = await nationalityRepo.FindAsync(
                    predicate: x => !x.IsDeleted,
                    disableTracking: true,
                    cancellationToken: cancellationToken);

                var nationalityChoices = nationalities
                    .Select(n => (n.Id, n.NameAr, n.NameEn))
                    .ToList();

                // Buffer Excel streams so header validation / parse does not consume the upload stream.
                await using var passengerBuffer = TripPersonExcelImporter.BufferFormFile(dto.PassengerAttachment);
                await using var staffBuffer = TripPersonExcelImporter.BufferFormFile(dto.StaffAttachment);

                var passengerImport = TripPersonExcelImporter.ImportPassengers(
                    passengerBuffer, dto.PassengerAttachment.FileName, nationalityChoices);
                if (!passengerImport.Success)
                    return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, message: passengerImport.Error);

                var staffImport = TripPersonExcelImporter.ImportStaff(
                    staffBuffer, dto.StaffAttachment.FileName, nationalityChoices);
                if (!staffImport.Success)
                    return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, message: staffImport.Error);

                var mapped = Mapper.Map<Entities.Tracker.TripInformation>(dto);
                mapped.GovernorateId = GetGovernorateIdFromClaims();
                SetEntityCreatedBaseProperties(mapped);

                var floatingUnit = await _floatingUnitService.GetByIdAsync(dto.FloatingUnitId, cancellationToken);

                if (floatingUnit.Data is FloatingUnitDto floatingUnitDto)
                {
                    var floatingUnitCode = floatingUnitDto.Code;

                    TripInformationFilter filter = new()
                    {
                        FloatingUnitId = dto.FloatingUnitId,
                        IsDeleted = false
                    };

                    var exist = await GetAllFilterAsync(filter, cancellationToken);
                    var existDataCollection = exist.Data as ICollection<TripInformationDto>;

                    if (existDataCollection?.Count > 0)
                    {
                        var lastTrip = existDataCollection.LastOrDefault();
                        if (lastTrip != null && lastTrip.Code.StartsWith(floatingUnitCode))
                        {
                            var numericPart = lastTrip.Code[floatingUnitCode.Length..];
                            if (int.TryParse(numericPart, out int number))
                            {
                                number++;
                                mapped.Code = floatingUnitCode + number.ToString("D4");
                            }
                        }
                    }
                    else
                    {
                        mapped.Code = floatingUnitCode + "0001";
                    }
                }

                if (dto.PassengerAttachment != null)
                {
                    passengerBuffer.Position = 0;
                    var passengerFormFile = new FormFile(
                        passengerBuffer,
                        0,
                        passengerBuffer.Length,
                        dto.PassengerAttachment.Name,
                        dto.PassengerAttachment.FileName)
                    {
                        Headers = dto.PassengerAttachment.Headers,
                        ContentType = dto.PassengerAttachment.ContentType
                    };

                    string res = await _uploaderConfiguration.UploadFile(passengerFormFile, "TripInformation/Passengers", cancellationToken);

                    if (res != null)
                    {
                        if (UploadResponse(res) != null)
                            return UploadResponse(res);
                    }

                    mapped.PassengerAttachment = res;
                }

                if (dto.StaffAttachment != null)
                {
                    staffBuffer.Position = 0;
                    var staffFormFile = new FormFile(
                        staffBuffer,
                        0,
                        staffBuffer.Length,
                        dto.StaffAttachment.Name,
                        dto.StaffAttachment.FileName)
                    {
                        Headers = dto.StaffAttachment.Headers,
                        ContentType = dto.StaffAttachment.ContentType
                    };

                    string staffRes = await _uploaderConfiguration.UploadFile(staffFormFile, "TripInformation/Staffs", cancellationToken);

                    if (staffRes != null)
                    {
                        if (UploadResponse(staffRes) != null)
                            return UploadResponse(staffRes);
                    }

                    mapped.StaffAttachment = staffRes;
                }

                if (dto.OtherAttachment != null)
                {
                    foreach (var formFile in dto.OtherAttachment)
                    {
                        Guid guid = Guid.NewGuid();
                        var addAttachmentDto = new AddAttachmentDto
                        {
                            Id = guid.ToString(),
                            Path = formFile,
                            AttachType = "TripInformation/Other",
                        };

                        IFinalResult attach = await _attachService.AddAsync(addAttachmentDto, cancellationToken);

                        mapped.TripAttachments.Add(new Entities.Tracker.TripAttachment
                        {
                            AttachmentId = (string)attach.Data,
                            TripInformationId = mapped.Id
                        });
                    }
                }

                var governorateId = mapped.GovernorateId;

                foreach (var row in passengerImport.Rows)
                {
                    mapped.TripPassengers.Add(new Entities.Tracker.TripPassenger
                    {
                        Name = row.Name,
                        Job = row.Job,
                        Mobile = row.Mobile,
                        Email = row.Email,
                        Gender = row.Gender,
                        IDType = row.IDType,
                        Identity = row.Identity,
                        NationalityId = row.NationalityId,
                        TripInformationId = mapped.Id,
                        GovernorateId = governorateId,
                        IsDeleted = false
                    });
                }

                foreach (var row in staffImport.Rows)
                {
                    mapped.TripStaffs.Add(new Entities.Tracker.TripStaff
                    {
                        Name = row.Name,
                        Job = row.Job,
                        Mobile = row.Mobile,
                        Email = row.Email,
                        Gender = row.Gender,
                        IDType = row.IDType,
                        Identity = row.Identity,
                        NationalityId = row.NationalityId,
                        TripInformationId = mapped.Id,
                        GovernorateId = governorateId,
                        IsDeleted = false
                    });
                }

                mapped.PassengerNumber = mapped.TripPassengers.Count;
                mapped.StaffNumber = mapped.TripStaffs.Count;
                mapped.IsDeleted = false;

                UnitOfWork.Repository.Add(mapped);

                var rows = await UnitOfWork.SaveChangesAsync(cancellationToken);

                return ResponseResult.PostResult(mapped, status: HttpStatusCode.Created, message: HttpStatusCode.Created.ToString());
            }
            catch (Exception ex)
            {
                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, exception: ex,
                    message: MessagesConstants.AddError + ex.Message);
            }
        }
        public override async Task<IFinalResult> UpdateAsync(AddTripInformationDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityToUpdate = await UnitOfWork.Repository.GetAsync(cancellationToken, dto.Id);

                string currentPassengerAttachment = entityToUpdate.PassengerAttachment;
                string currentStaffAttachment = entityToUpdate.StaffAttachment;

                TripExcelImportResult passengerImport = null;
                TripExcelImportResult staffImport = null;
                MemoryStream passengerBuffer = null;
                MemoryStream staffBuffer = null;

                try
                {
                    // Validate Excel before mutating entity / uploading files.
                    if (dto.PassengerAttachment != null || dto.StaffAttachment != null)
                    {
                        var nationalityRepo = UnitOfWork.GetRepository<Nationality>();
                        var nationalities = await nationalityRepo.FindAsync(
                            predicate: x => !x.IsDeleted,
                            disableTracking: true,
                            cancellationToken: cancellationToken);

                        var nationalityChoices = nationalities
                            .Select(n => (n.Id, n.NameAr, n.NameEn))
                            .ToList();

                        if (dto.PassengerAttachment != null)
                        {
                            passengerBuffer = TripPersonExcelImporter.BufferFormFile(dto.PassengerAttachment);
                            passengerImport = TripPersonExcelImporter.ImportPassengers(
                                passengerBuffer, dto.PassengerAttachment.FileName, nationalityChoices);
                            if (!passengerImport.Success)
                                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, message: passengerImport.Error);
                        }

                        if (dto.StaffAttachment != null)
                        {
                            staffBuffer = TripPersonExcelImporter.BufferFormFile(dto.StaffAttachment);
                            staffImport = TripPersonExcelImporter.ImportStaff(
                                staffBuffer, dto.StaffAttachment.FileName, nationalityChoices);
                            if (!staffImport.Success)
                                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, message: staffImport.Error);
                        }
                    }

                    var newEntity = Mapper.Map(dto, entityToUpdate);
                    newEntity.GovernorateId = GetGovernorateIdFromClaims();
                    SetEntityModifiedBaseProperties(newEntity);

                    if (IsSuperAdmin() && entityToUpdate.IsDeleted)
                        newEntity.IsDeleted = false;

                    if (dto.PassengerAttachment != null)
                    {
                        passengerBuffer.Position = 0;
                        var passengerFormFile = new FormFile(
                            passengerBuffer,
                            0,
                            passengerBuffer.Length,
                            dto.PassengerAttachment.Name,
                            dto.PassengerAttachment.FileName)
                        {
                            Headers = dto.PassengerAttachment.Headers,
                            ContentType = dto.PassengerAttachment.ContentType
                        };

                        string res = await _uploaderConfiguration.UploadFile(passengerFormFile, "TripInformation/Passengers", cancellationToken);

                        if (res != null && UploadResponse(res) != null)
                            return UploadResponse(res);

                        _uploaderConfiguration.DeleteFile(currentPassengerAttachment);
                        newEntity.PassengerAttachment = res;

                        var passengerRepo = UnitOfWork.GetRepository<Entities.Tracker.TripPassenger>();
                        var existingPassengers = await passengerRepo.FindAsync(
                            predicate: x => x.TripInformationId == entityToUpdate.Id,
                            disableTracking: false,
                            cancellationToken: cancellationToken);
                        if (existingPassengers != null)
                            passengerRepo.RemoveRange(existingPassengers);

                        var newPassengers = passengerImport.Rows.Select(row => new Entities.Tracker.TripPassenger
                        {
                            Name = row.Name,
                            Job = row.Job,
                            Mobile = row.Mobile,
                            Email = row.Email,
                            Gender = row.Gender,
                            IDType = row.IDType,
                            Identity = row.Identity,
                            NationalityId = row.NationalityId,
                            TripInformationId = entityToUpdate.Id,
                            GovernorateId = newEntity.GovernorateId,
                            IsDeleted = false
                        }).ToList();
                        passengerRepo.AddRange(newPassengers);
                        newEntity.PassengerNumber = newPassengers.Count;
                    }
                    else
                    {
                        newEntity.PassengerAttachment = currentPassengerAttachment;
                    }

                    if (dto.StaffAttachment != null)
                    {
                        staffBuffer.Position = 0;
                        var staffFormFile = new FormFile(
                            staffBuffer,
                            0,
                            staffBuffer.Length,
                            dto.StaffAttachment.Name,
                            dto.StaffAttachment.FileName)
                        {
                            Headers = dto.StaffAttachment.Headers,
                            ContentType = dto.StaffAttachment.ContentType
                        };

                        string staffRes = await _uploaderConfiguration.UploadFile(staffFormFile, "TripInformation/Staffs", cancellationToken);

                        if (staffRes != null && UploadResponse(staffRes) != null)
                            return UploadResponse(staffRes);

                        _uploaderConfiguration.DeleteFile(currentStaffAttachment);
                        newEntity.StaffAttachment = staffRes;

                        var staffRepo = UnitOfWork.GetRepository<Entities.Tracker.TripStaff>();
                        var existingStaff = await staffRepo.FindAsync(
                            predicate: x => x.TripInformationId == entityToUpdate.Id,
                            disableTracking: false,
                            cancellationToken: cancellationToken);
                        if (existingStaff != null)
                            staffRepo.RemoveRange(existingStaff);

                        var newStaff = staffImport.Rows.Select(row => new Entities.Tracker.TripStaff
                        {
                            Name = row.Name,
                            Job = row.Job,
                            Mobile = row.Mobile,
                            Email = row.Email,
                            Gender = row.Gender,
                            IDType = row.IDType,
                            Identity = row.Identity,
                            NationalityId = row.NationalityId,
                            TripInformationId = entityToUpdate.Id,
                            GovernorateId = newEntity.GovernorateId,
                            IsDeleted = false
                        }).ToList();
                        staffRepo.AddRange(newStaff);
                        newEntity.StaffNumber = newStaff.Count;
                    }
                    else
                    {
                        newEntity.StaffAttachment = currentStaffAttachment;
                    }

                    // Other attachments: append (unchanged).
                    if (dto.OtherAttachment != null)
                    {
                        foreach (var formFile in dto.OtherAttachment)
                        {
                            Guid guid = Guid.NewGuid();
                            var addAttachmentDto = new AddAttachmentDto
                            {
                                Id = guid.ToString(),
                                Path = formFile,
                                AttachType = "TripInformation/Other",
                            };

                            IFinalResult attach = await _attachService.AddAsync(addAttachmentDto, cancellationToken);

                            newEntity.TripAttachments.Add(new Entities.Tracker.TripAttachment
                            {
                                AttachmentId = (string)attach.Data,
                                TripInformationId = entityToUpdate.Id
                            });
                        }
                    }

                    UnitOfWork.Repository.Update(entityToUpdate, newEntity);

                    var affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);

                    if (affectedRows > 0)
                    {
                        Result = ResponseResult.PostResult(result: true, status: HttpStatusCode.Accepted,
                            message: MessagesConstants.UpdateSuccess);
                    }

                    return Result;
                }
                finally
                {
                    if (passengerBuffer != null)
                        await passengerBuffer.DisposeAsync();
                    if (staffBuffer != null)
                        await staffBuffer.DisposeAsync();
                }
            }
            catch (Exception ex)
            {
                return ResponseResult.PostResult(result: null, status: HttpStatusCode.BadRequest, exception: ex,
                    message: MessagesConstants.UpdateError + ex.Message);
            }
        }
        public override async Task<IFinalResult> DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityToDelete = await UnitOfWork.Repository.GetAsync(cancellationToken, id);

                _uploaderConfiguration.DeleteFile(entityToDelete.PassengerAttachment);
                _uploaderConfiguration.DeleteFile(entityToDelete.StaffAttachment);

                UnitOfWork.Repository.Remove(entityToDelete);
                var affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);
                if (affectedRows > 0)
                {
                    Result = ResponseResult.PostResult(result: true, status: HttpStatusCode.Accepted,
                        message: MessagesConstants.DeleteSuccess);
                }

                return Result;
            }
            catch (Exception ex)
            {
                return ResponseResult.PostResult(result: false, status: HttpStatusCode.BadRequest, exception: ex,
                    message: MessagesConstants.DeleteError + ex.Message);
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
