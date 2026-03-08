using Asp.Versioning;
using DeputyOffice.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Tracker.MarinaOrganization;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Reports.TouristMarina;
using SonoTracker.Common.DTO.Tracker.MarinaOrganization;
using SonoTracker.Common.DTO.Tracker.MarinaOrganization.Parameters;
using System.Net;
using System.Net.Mime;
using System.Threading;

namespace SonoTracker.Api.Controllers.V1.Tracker.Marina
{
    /// <summary>
    /// Constructor
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class MarinaOrganizationController(IMarinaOrganizationService marinaOrganizationService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>

        [HttpGet("get/{id}")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        public async Task<IFinalResult> GetAsync(Guid id, CancellationToken cancellationToken = default)
                                        => await marinaOrganizationService.GetByIdAsync(id, cancellationToken);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>

        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id, CancellationToken cancellationToken = default)
                                        => await marinaOrganizationService.GetByIdForEditAsync(id, cancellationToken);

        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>

        [HttpGet("getAll")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IFinalResult>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IFinalResult res = await marinaOrganizationService.GetAllAsync(cancellationToken: cancellationToken);

            if (res.Status == HttpStatusCode.NotFound) return NotFound(res);

            return Ok(res);
        }

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagingResult>> GetPagedAsync([FromBody] BaseParam<MarinaOrganizationFilter> filter, CancellationToken cancellationToken = default)
        {
            PagingResult res = await marinaOrganizationService.GetAllPagedAsync(filter, cancellationToken);

            if (res.Status == HttpStatusCode.NotFound) return NotFound(res);

            return Ok(res);
        }

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpPost("add")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status201Created)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IFinalResult>> AddAsync([FromBody] AddMarinaOrganizationDto dto, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await marinaOrganizationService.AddAsync(dto, cancellationToken);

            if (res.Status == HttpStatusCode.BadRequest) return BadRequest(res);

            if (res.Status == HttpStatusCode.Conflict) return Conflict(res);

            return Created("", res);
        }

        /// <summary>
        /// Get All Data paged For Drop Down
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getDropDown")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagingResult>> GetDropDownAsync([FromBody] BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            PagingResult res = await marinaOrganizationService.GetDropDownAsync(filter, cancellationToken);

            if (res.Status == HttpStatusCode.NotFound) return NotFound(res);

            return Ok(res);
        }

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpPut("update")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status202Accepted)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IFinalResult>> UpdateAsync([FromBody] AddMarinaOrganizationDto model, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await marinaOrganizationService.UpdateAsync(model, cancellationToken);

            if (res.Status == HttpStatusCode.BadRequest) return BadRequest(res);

            if (res.Status == HttpStatusCode.Conflict) return Conflict(res);

            return Accepted(res);
        }

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpDelete("delete/{id}")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status202Accepted)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IFinalResult>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await marinaOrganizationService.DeleteAsync(id, cancellationToken);

            if (res.Status == HttpStatusCode.BadRequest) return BadRequest(res);

            return Accepted(res);
        }

        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpDelete("deleteSoft/{id}")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status202Accepted)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IFinalResult>> DeleteSoftAsync(Guid id, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await marinaOrganizationService.DeleteSoftAsync(id, cancellationToken);

            if (res.Status == HttpStatusCode.BadRequest) return BadRequest(res);

            return Accepted(res);
        }

        /// <summary>
        /// Generates a project report based on the provided filter.
        /// </summary>
        [HttpGet("GetReport")]
        public async Task<IActionResult> GetProjectReportData([FromQuery] FilterTouristMarinaReportDto filter, CancellationToken cancellationToken = default)
        {
            var report = await marinaOrganizationService.GenerateReportAsync(filter, cancellationToken);
            return File(report, MediaTypeNames.Application.Pdf, ReportHelper.GetReportDetails(filter.ReportName, filter.ReportType));
        }
    }
}
