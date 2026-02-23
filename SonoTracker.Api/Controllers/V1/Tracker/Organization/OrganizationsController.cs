using DeputyOffice.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Tracker.Organization;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Reports.Org;
using SonoTracker.Common.DTO.Tracker.Organization;
using SonoTracker.Common.DTO.Tracker.Organization.Parameters;
using System.Net;
using System.Net.Mime;

namespace SonoTracker.Api.Controllers.V1.Tracker.Organization
{
    /// <summary>
    /// Constructor
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationsController(IOrganizationService organizationService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IFinalResult> GetAsync(Guid id) => await organizationService.GetByIdAsync(id);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) => await organizationService.GetByIdForEditAsync(id);

        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await organizationService.GetAllAsync();

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<OrganizationFilter> filter) => await organizationService.GetAllPagedAsync(filter);

        /// <summary>
        /// Get All Data paged For Drop Down
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getDropDown")]
        public async Task<PagingResult> GetDropDownAsync([FromBody] BaseParam<SearchCriteriaFilter> filter) => await organizationService.GetDropDownAsync(filter);

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult<IFinalResult>> AddAsync([FromForm] AddOrganizationDto dto)
        {
            var model = await organizationService.AddAsync(dto);

            if (model.Status == HttpStatusCode.Conflict)
            {
                return BadRequest(model);
            }
            if (model.Status == HttpStatusCode.BadRequest)
            {
                return BadRequest(model);
            }

            return Ok(model);
        }

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult<IFinalResult>> UpdateAsync([FromForm] AddOrganizationDto model)
        {
            var entity = await organizationService.UpdateAsync(model);

            if (entity.Status == HttpStatusCode.Conflict)
            {
                return BadRequest(entity);
            }
            if (entity.Status == HttpStatusCode.BadRequest)
            {
                return BadRequest(entity);
            }

            return Ok(entity);
        }
        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<IFinalResult>> DeleteAsync(Guid id)
        {
            var model = await organizationService.DeleteAsync(id);

            if (model.Status == HttpStatusCode.NotFound)
            {
                return BadRequest("هذا البيان غير موجود");
            }

            return Ok(model);
        }

        /// <summary>
        /// Soft Remove by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<ActionResult<IFinalResult>> DeleteSoftAsync(Guid id)
        {
            var model = await organizationService.DeleteSoftAsync(id);

            if (model.Status == HttpStatusCode.NotFound)
            {
                return BadRequest("هذا البيان غير موجود");
            }

            return Ok(model);
        }

        /// <summary>
        /// Remove Range by Organization Ids
        /// </summary>
        /// <param name="ids">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteRange")]
        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids) => await organizationService.DeleteRangeAsync(ids);

        /// <summary>
        /// Generates a project report based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter containing report parameters.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>A PDF file containing the generated report.</returns>
        [HttpGet("GetReport")]
        public async Task<IActionResult> GetProjectReportData([FromQuery] FilterOrgReportDTO filter, CancellationToken cancellationToken = default)
        {
            var report = await organizationService.GenerateReportAsync(filter, cancellationToken);
            return File(report, MediaTypeNames.Application.Pdf, ReportHelper.GetReportDetails(filter.ReportName, filter.ReportType));
        }

    }
}
