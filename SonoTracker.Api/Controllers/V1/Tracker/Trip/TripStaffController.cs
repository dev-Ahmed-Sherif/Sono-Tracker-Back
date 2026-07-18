using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Tracker.TripStaff;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripStaff;
using SonoTracker.Common.DTO.Tracker.TripStaff.Parameters;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Api.Controllers.V1.Tracker.Trip
{
    /// <summary>
    /// Trip staff endpoints
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class TripStaffController(ITripStaffService tripStaffService) : BaseController
    {
        /// <summary>
        /// Get By Id
        /// </summary>
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status200OK)]
        public async Task<IFinalResult> GetAsync(string id, CancellationToken cancellationToken = default)
                                        => await tripStaffService.GetByIdAsync(id, cancellationToken);

        /// <summary>
        /// Get For Edit
        /// </summary>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(string id, CancellationToken cancellationToken = default)
                                        => await tripStaffService.GetByIdForEditAsync(id, cancellationToken);

        /// <summary>
        /// Get All
        /// </summary>
        [HttpGet("getAll")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<IFinalResult>> GetAllAsync([FromQuery] string? tripInformationId, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await tripStaffService.GetAllAsync(tripInformationId, cancellationToken);
            return Ok(res);
        }

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        [HttpPost("getPaged")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagingResult>> GetPagedAsync([FromBody] BaseParam<TripStaffFilter> filter, CancellationToken cancellationToken = default)
        {
            PagingResult res = await tripStaffService.GetAllPagedAsync(filter, cancellationToken);
            return Ok(res);
        }

        /// <summary>
        /// Get All Data paged For Drop Down
        /// </summary>
        [HttpPost]
        [Route("getDropDown")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagingResult>> GetDropDownAsync([FromBody] BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            PagingResult res = await tripStaffService.GetDropDownAsync(filter, cancellationToken);
            return Ok(res);
        }

        /// <summary>
        /// Add
        /// </summary>
        [HttpPost("add")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IFinalResult>> AddAsync([FromBody] AddTripStaffDto dto, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await tripStaffService.AddAsync(dto, cancellationToken);

            if (res.Status == HttpStatusCode.BadRequest) return BadRequest(res);

            if (res.Status == HttpStatusCode.Conflict) return Conflict(res);

            return Created("", res);
        }

        /// <summary>
        /// Update
        /// </summary>
        [HttpPut("update")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IFinalResult>> UpdateAsync([FromBody] AddTripStaffDto model, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await tripStaffService.UpdateAsync(model, cancellationToken);

            if (res.Status == HttpStatusCode.BadRequest) return BadRequest(res);

            if (res.Status == HttpStatusCode.Conflict) return Conflict(res);

            return Accepted(res);
        }

        /// <summary>
        /// Remove by id
        /// </summary>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IFinalResult>> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await tripStaffService.DeleteAsync(id, cancellationToken);

            if (res.Status == HttpStatusCode.BadRequest) return BadRequest(res);

            return Accepted(res);
        }

        /// <summary>
        /// Soft Remove by id
        /// </summary>
        [HttpDelete("deleteSoft/{id}")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IFinalResult>> DeleteSoftAsync(string id, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await tripStaffService.DeleteSoftAsync(id, cancellationToken);

            if (res.Status == HttpStatusCode.BadRequest) return BadRequest(res);

            return Accepted(res);
        }

        /// <summary>
        /// Remove Range by TripStaff Ids
        /// </summary>
        [HttpDelete("deleteRange")]
        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
                                        => await tripStaffService.DeleteRangeAsync(ids, cancellationToken);
    }
}
