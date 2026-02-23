using Microsoft.AspNetCore.Mvc;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.GovernorateGeoPoint.Parameters;
using SonoTracker.Common.DTO.Tracker.GovernorateGeoPoint;
using SonoTracker.Application.Services.Tracker.GovernorateGeoPoint;
using SonoTracker.Api.Controllers.V1.Base;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace SonoTracker.Api.Controllers.V1.Tracker.Governorate
{
    /// <summary>
    /// Constructor
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class GovernorateGeoPointsController(IGovernorateGeoPointService governorateGeoPointService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IFinalResult> GetAsync(Guid id) 
                     => await governorateGeoPointService.GetByIdAsync(id);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) 
                     => await governorateGeoPointService.GetByIdForEditAsync(id);

        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() 
                     => await governorateGeoPointService.GetAllAsync();

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<GovernorateGeoPointFilter> filter) 
                     => await governorateGeoPointService.GetAllPagedAsync(filter);

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IFinalResult> AddAsync([FromBody] AddGovernorateGeoPointDto dto) 
                     => await governorateGeoPointService.AddAsync(dto);

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IFinalResult> UpdateAsync(AddGovernorateGeoPointDto model) 
                     => await governorateGeoPointService.UpdateAsync(model);

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IFinalResult> DeleteAsync(Guid id) 
                     => await governorateGeoPointService.DeleteAsync(id);

        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<IFinalResult> DeleteSoftAsync(Guid id) 
                     => await governorateGeoPointService.DeleteSoftAsync(id);
    }
}

