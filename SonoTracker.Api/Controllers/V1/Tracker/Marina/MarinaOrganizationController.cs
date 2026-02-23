using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Tracker.Governorate;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TouristMarina.Parameters;
using SonoTracker.Common.DTO.Tracker.TouristMarina;
using SonoTracker.Application.Services.Tracker.MarinaOrganization;
using SonoTracker.Application.Services.Tracker.TouristMarina;
using SonoTracker.Common.DTO.Tracker.MarinaOrganization.Parameters;
using SonoTracker.Common.DTO.Tracker.MarinaOrganization;
using DeputyOffice.Common.Helpers;
using SonoTracker.Application.Services.Tracker.Organization;
using SonoTracker.Common.DTO.Reports.Org;
using System.Net.Mime;
using SonoTracker.Common.DTO.Reports.TouristMarina;
using Microsoft.AspNetCore.Authorization;

namespace SonoTracker.Api.Controllers.V1.Tracker.Marina
{
    /// <summary>
    /// Constructor
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MarinaOrganizationController(IMarinaOrganizationService marinaOrganizationService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IFinalResult> GetAsync(Guid id) => await marinaOrganizationService.GetByIdAsync(id);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) => await marinaOrganizationService.GetByIdForEditAsync(id);

        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await marinaOrganizationService.GetAllAsync();

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<MarinaOrganizationFilter> filter) => await marinaOrganizationService.GetAllPagedAsync(filter);

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IFinalResult> AddAsync([FromBody] AddMarinaOrganizationDto dto)
        {
            //var gov = await _GovernorateService.GetByIdAsync(dto.GovId);
            var res = await marinaOrganizationService.AddAsync(dto);
            return res;
        }

        /// <summary>
        /// Get All Data paged For Drop Down
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getDropDown")]
        public async Task<PagingResult> GetDropDownAsync([FromBody] BaseParam<SearchCriteriaFilter> filter) => await marinaOrganizationService.GetDropDownAsync(filter);

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IFinalResult> UpdateAsync(AddMarinaOrganizationDto model) => await marinaOrganizationService.UpdateAsync(model);

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IFinalResult> DeleteAsync(Guid id) => await marinaOrganizationService.DeleteAsync(id);


        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<IFinalResult> DeleteSoftAsync(Guid id) => await marinaOrganizationService.DeleteSoftAsync(id);

        [HttpGet("GetReport")]
        public async Task<IActionResult> GetProjectReportData([FromQuery] FilterTouristMarinaReportDto filter, CancellationToken cancellationToken = default)
        {
            var report = await marinaOrganizationService.GenerateReportAsync(filter, cancellationToken);
            return File(report, MediaTypeNames.Application.Pdf, ReportHelper.GetReportDetails(filter.ReportName, filter.ReportType));
        }
    }
}
