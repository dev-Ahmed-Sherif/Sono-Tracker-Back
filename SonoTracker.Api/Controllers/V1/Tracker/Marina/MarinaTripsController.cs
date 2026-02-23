using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.MarinaTrip.Parameters;
using SonoTracker.Common.DTO.Tracker.MarinaTrip;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Tracker.MarinaTrip;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace SonoTracker.Api.Controllers.V1.Tracker.Marina
{

    /// <summary>
    /// Constructor
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class MarinaTripsController(IMarinaTripService marinaTripService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IFinalResult> GetAsync(Guid id) => await marinaTripService.GetByIdAsync(id);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) => await marinaTripService.GetByIdForEditAsync(id);


        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await marinaTripService.GetAllAsync();

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<MarinaTripFilter> filter) => await marinaTripService.GetAllPagedAsync(filter);

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IFinalResult> AddAsync([FromBody] AddMarinaTripDto dto) => await marinaTripService.AddAsync(dto);

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IFinalResult> UpdateAsync(AddMarinaTripDto model) => await marinaTripService.UpdateAsync(model);

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IFinalResult> DeleteAsync(Guid id) => await marinaTripService.DeleteAsync(id);

        /// <summary>
        /// Soft Remove by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<IFinalResult> DeleteSoftAsync(Guid id) => await marinaTripService.DeleteSoftAsync(id);

        /// <summary>
        /// Remove Range by Organization Ids
        /// </summary>
        /// <param name="ids">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteRange")]
        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids) => await marinaTripService.DeleteRangeAsync(ids);
    }
}
