using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TouristMarina;
using SonoTracker.Common.DTO.Tracker.TripInformation.Parameters;
using SonoTracker.Common.DTO.Tracker.TripInformation;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Common.DTO.Tracker.MarinaOrganization.Parameters;
using SonoTracker.Common.DTO.Tracker.MarinaOrganization;
using SonoTracker.Common.DTO.Tracker.TouristMarina.Parameters;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Common.DTO.Lookup.Town;
using SonoTracker.Common.DTO.Reports.TouristMarina;
using SonoTracker.Common.DTO.Reports.Org;
using Microsoft.Reporting.NETCore;
using SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization.Parameters;
using SonoTracker.Domain.Enum;
using System.Reflection;
using System.Threading;

namespace SonoTracker.Application.Services.Tracker.MarinaOrganization
{
    public class MarinaOrganizationService : BaseService<Domain.Entities.Tracker.MarinaOrganization, AddMarinaOrganizationDto, EditMarinaOrganizationDto, MarinaOrganizationDto, string, string>, IMarinaOrganizationService
    {
        public MarinaOrganizationService(IServiceBaseParameter<Entities.Tracker.MarinaOrganization> businessBaseParameter) : base(businessBaseParameter)
        {

        }
        public override async Task<IFinalResult> GetByIdForEditAsync(object id)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                .Include(t => t.Organization)
               .Include(x => x.TouristMarina)
                );
            var mapped = Mapper.Map<Domain.Entities.Tracker.MarinaOrganization, EditMarinaOrganizationDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                .Include(t => t.Organization)
               .Include(x => x.TouristMarina));
            var mapped = Mapper.Map<Domain.Entities.Tracker.MarinaOrganization, MarinaOrganizationDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }
        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.MarinaOrganization, bool>> predicate = null)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(include: src => src
             .Include(t => t.Organization)
             .Include(x => x.TouristMarina));
            var filteredEntities = entity.Where(e => !e.IsDeleted);
            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.MarinaOrganization>, IEnumerable<MarinaOrganizationDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }
        public async Task<PagingResult> GetAllPagedAsync(BaseParam<MarinaOrganizationFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), 
                    pageNumber: offset, 
                    pageSize: limit, 
                    filter.OrderByValue,
                    include: src => src
                    .Include(t => t.Organization)
                    .Include(x => x.TouristMarina)
                    );

            var data = Mapper.Map<IEnumerable<Entities.Tracker.MarinaOrganization>, IEnumerable<MarinaOrganizationDto>>(query.Item2);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter)
        {

            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit);

            var data = Mapper.Map<IEnumerable<Entities.Tracker.MarinaOrganization>, IEnumerable<MarinaOrganizationDto>>(query.Item2);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);

        }


        static Expression<Func<Entities.Tracker.MarinaOrganization, bool>> PredicateBuilderFunction(MarinaOrganizationFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.MarinaOrganization>(x => x.IsDeleted == filter.IsDeleted);

            //if (filter.TownId.HasValue)
            //{
            //    predicate = predicate.And(x => x.TownId == filter.TownId.Value);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.Name))
            //{
            //    predicate = predicate.And(x => x.Name.Contains(filter.Name));
            //}
            //if (!string.IsNullOrWhiteSpace(filter.Code))
            //{
            //    predicate = predicate.And(x => x.Code.Contains(filter.Code));
            //}

            return predicate;
        }
        static Expression<Func<Entities.Tracker.MarinaOrganization, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.MarinaOrganization>(true);
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


        static Expression<Func<Entities.Tracker.MarinaOrganization, bool>> PredicateBuilderReportFunction(FilterTouristMarinaReportDto filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.MarinaOrganization>(x => x.IsDeleted == filter.IsDeleted);

            if (!string.IsNullOrWhiteSpace(filter.TownId))
            {
                predicate = predicate.And(x => x.TouristMarina.Town.Id == filter.TownId);
            }
            if (!string.IsNullOrEmpty(filter.TouristMarinaId))
            {
                predicate = predicate.And(x => x.TouristMarina.Id == filter.TouristMarinaId);
            }
            if (!string.IsNullOrEmpty(filter.OrganizationId))
            {
                predicate = predicate.And(x => x.Organization.Id == filter.OrganizationId);
            }

            return predicate;
        }


        public async Task<IFinalResult> GetAllReportAsync(FilterTouristMarinaReportDto filter)
        {

            var query = await UnitOfWork.Repository.FindAsync(predicate: PredicateBuilderReportFunction(filter),
                 include: src => src
                   .Include(x => x.TouristMarina)
                     .Include(t => t.Organization)
                     .Include(x => x.TouristMarina.Town)
                     .Include(x => x.TouristMarina.GeoPoint)
                );


            var data = Mapper.Map<IEnumerable<Domain.Entities.Tracker.MarinaOrganization>, IEnumerable<TouristMarinaReportDto>>(query);
            return ResponseResult.PostResult(data, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<byte[]> GenerateReportAsync(FilterTouristMarinaReportDto filter, CancellationToken cancellationToken = default)
        {
            // get report file
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("SonoTracker.Application.dll", string.Empty);
            //string fileDirPath = "D:\\Projects\\SonoTracker\\Tracker\\SonoTracker.Application\\";
            //string fileDirPath = "D:\\Projects\\SonoTracker\\Tracker\\SonoTracker.Api\\bin\\Debug\\net8.0\\";
            //"D:\Projects\SonoTracker\Tracker\SonoTracker.Api\bin\Debug\net8.0\ReportsFiles";
            // Fixing unrecognized escape sequence by using verbatim string literals (prefixing with @)
            //string fileDirPath = @"D:\Projects\SonoTracker\Tracker\SonoTracker.Api\bin\Debug\net8.0\ReportsFiles";
            Console.WriteLine(string.Format(@"{0}\{1}.rdlc", fileDirPath, filter.ReportName));
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

            if (filter.ReportName == "TouristMarinasReport")
            {
                Org = await GetAllReportAsync(filter);
                var orgData = Org.Data as IEnumerable<TouristMarinaReportDto>;
                if (orgData == null)
                {
                    throw new InvalidOperationException("No data found for the report.");
                }

                //Add data source to the report


                report.DataSources.Add(new ReportDataSource() { Name = "TouristMarina", Value = orgData });
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

    }
}
