using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Tracker.TripPassenger;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripPassenger;
using SonoTracker.Common.DTO.Tracker.TripPassenger.Parameters;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Api.Controllers.V1.Tracker.Trip
{
    /// <summary>
    /// Trip passenger endpoints
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class TripPassengerController(ITripPassengerService tripPassengerService) : BaseController
    {
        /// <summary>
        /// Get By Id
        /// </summary>
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status200OK)]
        public async Task<IFinalResult> GetAsync(string id, CancellationToken cancellationToken = default)
                                        => await tripPassengerService.GetByIdAsync(id, cancellationToken);

        /// <summary>
        /// Get For Edit
        /// </summary>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(string id, CancellationToken cancellationToken = default)
                                        => await tripPassengerService.GetByIdForEditAsync(id, cancellationToken);

        /// <summary>
        /// Get All
        /// </summary>
        [HttpGet("getAll")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<IFinalResult>> GetAllAsync([FromQuery] string? tripInformationId, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await tripPassengerService.GetAllAsync(tripInformationId, cancellationToken);
            return Ok(res);
        }

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        [HttpPost("getPaged")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagingResult>> GetPagedAsync([FromBody] BaseParam<TripPassengerFilter> filter, CancellationToken cancellationToken = default)
        {
            PagingResult res = await tripPassengerService.GetAllPagedAsync(filter, cancellationToken);
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
            PagingResult res = await tripPassengerService.GetDropDownAsync(filter, cancellationToken);
            return Ok(res);
        }

        /// <summary>
        /// Add
        /// </summary>
        [HttpPost("add")]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IFinalResult), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IFinalResult>> AddAsync([FromBody] AddTripPassengerDto dto, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await tripPassengerService.AddAsync(dto, cancellationToken);

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
        public async Task<ActionResult<IFinalResult>> UpdateAsync([FromBody] AddTripPassengerDto model, CancellationToken cancellationToken = default)
        {
            IFinalResult res = await tripPassengerService.UpdateAsync(model, cancellationToken);

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
            IFinalResult res = await tripPassengerService.DeleteAsync(id, cancellationToken);

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
            IFinalResult res = await tripPassengerService.DeleteSoftAsync(id, cancellationToken);

            if (res.Status == HttpStatusCode.BadRequest) return BadRequest(res);

            return Accepted(res);
        }

        /// <summary>
        /// Remove Range by TripPassenger Ids
        /// </summary>
        [HttpDelete("deleteRange")]
        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
                                        => await tripPassengerService.DeleteRangeAsync(ids, cancellationToken);
    }
}
