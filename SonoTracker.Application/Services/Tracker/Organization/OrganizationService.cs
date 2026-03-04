using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Application.Services.Tracker.FloatingUnitOrganization;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Identity.User;
using SonoTracker.Common.DTO.Reports.Org;
using SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization.Parameters;
using SonoTracker.Common.DTO.Tracker.Organization;
using SonoTracker.Common.DTO.Tracker.Organization.Parameters;
using SonoTracker.Common.Helpers.MediaUploader;
using SonoTracker.Domain;
using SonoTracker.Domain.Enum;

namespace SonoTracker.Application.Services.Tracker.Organization
{
    public class OrganizationService : BaseService<Domain.Entities.Tracker.Organization, AddOrganizationDto, EditOrganizationDto, OrganizationDto, string, string>, IOrganizationService
    {
        private readonly UserData _user;
        private readonly IFloatingUnitOrganizationService _floatingUnitOrganizationService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _request;
        private readonly UploaderConfiguration _uploaderConfiguration;
        public OrganizationService(
            IServiceBaseParameter<Domain.Entities.Tracker.Organization> businessBaseParameter, UserData user,
            IFloatingUnitOrganizationService floatingUnitOrganizationService,
            IWebHostEnvironment hostingEnvironment, IHttpContextAccessor request) : base(businessBaseParameter)
        {
            _user = user;
            _floatingUnitOrganizationService = floatingUnitOrganizationService;
            _hostingEnvironment = hostingEnvironment;
            _request = request;
            _uploaderConfiguration = new UploaderConfiguration(_hostingEnvironment, _request);
        }
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.Organization, bool>> predicate = null)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
             .Include(t => t.Nationality)
             .Include(x => x.InspectionType));
            var filteredEntities = entity.Where(e => !e.IsDeleted);
            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.Organization>, IEnumerable<OrganizationDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<OrganizationFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                 include: src => src
               .Include(t => t.InspectionType)
               .Include(x => x.Nationality));

            var data = Mapper.Map<IEnumerable<Domain.Entities.Tracker.Organization>, IEnumerable<OrganizationDto>>(query.Result);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data,status:HttpStatusCode.OK, MessagesConstants.Success);
        }
        public override async Task<IFinalResult> GetByIdForEditAsync(object id)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                .Include(x=>x.InspectionType)
                .Include(x => x.Nationality));
            var mapped = Mapper.Map<Domain.Entities.Tracker.Organization, EditOrganizationDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> AddAsync(AddOrganizationDto model)
        {
            var IsExisted = await UnitOfWork.Repository.Any(x => 
                                                x.NameAr == model.NameAr &&
                                                x.NameEn == model.NameEn &&
                                                x.IsDeleted != true);

            if (IsExisted)
                return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict, 
                                            message: MessagesConstants.Existed);

            var entity = Mapper.Map<Entities.Tracker.Organization>(model);

            var data = await GetAllAsync();

            var dataCollection = data.Data as ICollection<OrganizationDto>;

            if (dataCollection.Count > 0)
            {
                if (int.TryParse(dataCollection.OrderByDescending(o => o.Code).FirstOrDefault().Code, out int num))
                {
                   int newCode = ++num;
                   entity.Code = newCode.ToString();
                }
            }
            else
            {
                entity.Code = "1";
            }

            if (model.ImageUrl != null)
            {
                string res = await _uploaderConfiguration
                                   .UploadFile(model.ImageUrl, "Organization");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                entity.ImageUrl = res;
            }

            var result = await UnitOfWork.Repository.AddAsync(entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync();

            if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.AddError);

            return ResponseResult.PostResult(result: entity.Id, HttpStatusCode.Created, null, MessagesConstants.AddSuccess);
        }
        public override async Task<IFinalResult> UpdateAsync(AddOrganizationDto model)
        {
            var IsExisted = await UnitOfWork.Repository.Any(x => 
                                        x.NameAr == model.NameAr &&
                                        x.NameEn == model.NameEn && 
                                        x.Id != model.Id && x.IsDeleted != true);

            if (IsExisted)
                return new ResponseResult().PostResult(result: false, status: HttpStatusCode.Conflict, message: MessagesConstants.Existed);

            Domain.Entities.Tracker.Organization entityToUpdate = await UnitOfWork.Repository.GetAsync(model.Id);

            var entity = Mapper.Map(model, entityToUpdate);

            if (model.ImageUrl != null)
            {
                string res = await _uploaderConfiguration
                                   .UploadFile(model.ImageUrl, "Organization");

                if (res != null)
                {
                    if (UploadResponse(res) != null)
                        return UploadResponse(res);
                }

                entity.ImageUrl = res;

                _uploaderConfiguration.DeleteFile(entityToUpdate.ImageUrl);
            }
            else
            {
                var entityExist = await GetByIdForEditAsync(model.Id);
                var entityRes = (EditOrganizationDto)entityExist.Data;
                entity.ImageUrl = entityRes.ImageUrl;
            }

            UnitOfWork.Repository.Update(entityToUpdate, entity);

            //SetEntityModifiedBaseProperties(entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync();

            if (affectedRows <= 0) return ResponseResult.PostResult(false, HttpStatusCode.BadRequest, null, MessagesConstants.UpdateError);

            return ResponseResult.PostResult(true, HttpStatusCode.OK, null, MessagesConstants.UpdateSuccess);
        }
        public override async Task<IFinalResult> DeleteAsync(object id)
        {
            try
            {
                var entityToDelete = await UnitOfWork.Repository.GetAsync(id);
                if (entityToDelete == null) 
                    return ResponseResult.PostResult(result: false, status: HttpStatusCode.NotFound,
                        message: MessagesConstants.DeleteError);
                // Remove Uploaded File
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
        static Expression<Func<Domain.Entities.Tracker.Organization, bool>> PredicateBuilderFunction(OrganizationFilter filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Tracker.Organization>(x => x.IsDeleted != true);
            //var predicate = PredicateBuilder.New<Domain.Entities.Tracker.Organization>();
            if (!string.IsNullOrWhiteSpace(filter?.NameAr))
            {
                predicate = predicate.And(x => x.NameAr.Contains(filter.NameAr));
            }
            if (!string.IsNullOrWhiteSpace(filter?.Code))
            {
                predicate = predicate.And(x => x.Code.Contains(filter.Code));
            }

            if (!string.IsNullOrWhiteSpace(filter?.NameEn))
            {
                predicate = predicate.And(x => x.NameEn.Contains(filter.NameEn));
            }
            if (filter.TouristMarinaNumber.HasValue)
            {
                predicate = predicate.And(x => x.TouristMarinaNumber == filter.TouristMarinaNumber.Value);
            }
            if (filter.OrganizationTypeId.HasValue)
            {
                predicate = predicate.And(x => x.OrganizationTypeId == filter.OrganizationTypeId.Value);
            }
            if (filter.AppliedOn.HasValue)
            {
                predicate = predicate.And(x => x.AppliedOn == filter.AppliedOn.Value);
            }

            return predicate;
        }
        //private async Task MapAttachmentInternalAsync(EditOrganizationDto mapped)
        //{

        //    if (mapped != null)
        //    {
        //        var tokens = await _fileRepository.GetTokens(mapped.OrganizationAttachments.Select(x => x.FileId).ToList());
        //        mapped.OrganizationAttachments.ForEach(e =>
        //        {
        //            var token = tokens.First(x => x.Id == e.FileId);
        //            e.Url = _urls.DownloadFileWithAppCode + "/" + e.FileId + "?token=" + token.Token;
        //        });
        //    }
        //}
        //TO-DO remove the loop here for finding the entities to be deleted
        // get all entities at once using search first (FindAsync)
        // then use remove range

        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids)
        {
            var idsList = ids.ToList();
            var entitiesToDelete = await UnitOfWork.Repository.FindAsync(d => idsList.Contains(d.Id));

            UnitOfWork.Repository.RemoveRange(entitiesToDelete);

            var rows = await UnitOfWork.SaveChangesAsync();

            return ResponseResult.PostResult(result: rows, status: HttpStatusCode.NoContent, message: MessagesConstants.DeleteSuccess);
        }

        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter)
        {

            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.Organization>, IEnumerable<OrganizationDto>>(query.Item2.Where(x => x.IsDeleted != true));

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }
        static Expression<Func<Entities.Tracker.Organization, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.Organization>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.NameAr.Contains(filter.SearchCriteria));
                predicate = predicate.Or(b => b.NameEn.Contains(filter.SearchCriteria));
                predicate = predicate.Or(b => b.Code.Contains(filter.SearchCriteria));
            }
            return predicate;
        }

        public async Task<IFinalResult> GetAllReportAsync(FilterOrgReportDTO filter)
        {
            var query = await UnitOfWork.Repository.FindAsync(predicate: PredicateBuilderReportFunction(filter),
                 include: src => src
                                 .Include(x => x.Nationality));

            var data = Mapper.Map<IEnumerable<Domain.Entities.Tracker.Organization>, IEnumerable<OrgReportDTO>>(query);
            
            return ResponseResult.PostResult(data, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        static Expression<Func<Domain.Entities.Tracker.Organization, bool>> PredicateBuilderReportFunction(FilterOrgReportDTO filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Tracker.Organization>(x => x.IsDeleted != true);

            if (filter.OrganizationIds.Length > 0)
            {
                predicate = predicate.And(e => filter.OrganizationIds.Any(id => id == e.Id));
            }
            if (!string.IsNullOrWhiteSpace(filter?.NameAr))
            {
                predicate = predicate.And(x => x.NameAr.Contains(filter.NameAr));
            }
            if (!string.IsNullOrWhiteSpace(filter?.NameEn))
            {
                predicate = predicate.And(x => x.NameEn.Contains(filter.NameEn));
            }
            if (filter.OrganizationTypeId.HasValue)
            {
                predicate = predicate.And(x => x.OrganizationTypeId == filter.OrganizationTypeId.Value);
            }
            

            return predicate;
        }

        public async Task<byte[]> GenerateReportAsync(FilterOrgReportDTO filter, CancellationToken cancellationToken = default)
        {
            // get report file
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("SonoTracker.Application.dll", string.Empty);
            Console.WriteLine(string.Format(@"{0}ReportsFiles\{1}.rdlc", fileDirPath, filter.ReportName));
            string rdclFilePath = string.Format(@"{0}ReportsFiles\{1}.rdlc", fileDirPath, filter.ReportName);


            // file encoding
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("utf-8");

            LocalReport report = new()
            {
                ReportPath = rdclFilePath
                //ReportPath = fileDirPath
            };

            // prepare data for report
            IFinalResult Org = null; // Initialize Org to avoid CS0165 error

            if (filter.ReportName == "BeneficiaryOrgReport" && filter.OrganizationTypeId== OrganizationType.GovernmentCompany)
            {
                Org = await GetAllReportAsync(filter);
                var orgData = Org.Data as IEnumerable<OrgReportDTO>;
                if (orgData == null)
                {
                    throw new InvalidOperationException("No data found for the report.");
                }

                for (int i = 0; i < orgData.ToList().Count; i++)
                {
                    orgData.ToList()[i].User = _user.Name;
                }

                //Add data source to the report
                report.DataSources.Add(new ReportDataSource() { Name = "BenificaryOrg", Value = orgData });
            }
            else if (filter.ReportName == "CompaniesOwningFloatingUnits" && filter.OrganizationTypeId == OrganizationType.OwnerCompany)
            {
                Org = await GetAllReportAsync(filter);

                var orgData = Org.Data as IEnumerable<OrgReportDTO> ?? 
                    throw new InvalidOperationException("No data found for the report.");

                for (int i = 0; i < orgData.Count(); i++)
                {
                    var org = _floatingUnitOrganizationService.GetAllPagedAsync(new BaseParam<FloatingUnitOrganizationFilter>
                    {
                        Filter = new FloatingUnitOrganizationFilter
                        {
                            OrganizationId = orgData.ElementAt(i).Id
                        }
                    }).Result;
                    // Check if TouristMarinaNumber is 0 and set it to null
                    //if (orgData.ElementAt(i).TouristMarinaNumber == 0)
                    //{
                    //    orgData.ElementAt(i).TouristMarinaNumber = null;
                    //}
                }
                //Add data source to the report
                report.DataSources.Add(new ReportDataSource() { Name = "BenificaryOrg", Value = orgData });
            }
            else if (filter.ReportName == "CompaniesOperatingFloatingUnits" && filter.OrganizationTypeId == OrganizationType.OperatingCompany)
            {
                Org = await GetAllReportAsync(filter);
                var orgData = Org.Data as IEnumerable<OrgReportDTO> ?? throw new InvalidOperationException("No data found for the report.");
                for (int i = 0; i < orgData.Count(); i++)
                {
                    var org = _floatingUnitOrganizationService.GetAllPagedAsync(new BaseParam<FloatingUnitOrganizationFilter>
                    {
                        Filter = new FloatingUnitOrganizationFilter
                        {
                            OrganizationId = orgData.ElementAt(i).Id
                        }
                    }).Result;
                }
                //Add data source to the report
                report.DataSources.Add(new ReportDataSource() { Name = "BenificaryOrg", Value = orgData });
            }
          


            if (Org == null || Org.Data == null)
            {
                throw new InvalidOperationException("Failed to retrieve report data.");
            }
            
            byte[] renderedBytes = [];
            try
            {
                renderedBytes = report.Render(filter.ReportType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new InvalidOperationException("Error rendering report: " + ex.Message, ex);
            }

            //byte[] renderedBytes = report.Render("");

            return renderedBytes;
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

