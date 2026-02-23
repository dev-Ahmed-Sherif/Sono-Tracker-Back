using Microsoft.AspNetCore.Mvc;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Application.Services.Tracker.FloatingUnit;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Common.DTO.Tracker.FloatingUnit.Parameters;
using SonoTracker.Common.DTO.Tracker.FloatingUnit;
using Microsoft.AspNetCore.Authorization;

namespace SonoTracker.Api.Controllers.V1.Tracker.FloatingUnit
{
    /// <summary>
    /// Constructor
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FloatingUnitsController(IFloatingUnitService floatingUnitService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IFinalResult> GetAsync(Guid id) => await floatingUnitService.GetByIdAsync(id);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) => await floatingUnitService.GetByIdForEditAsync(id);

        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await floatingUnitService.GetAllAsync();

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<FloatingUnitFilter> filter) => await floatingUnitService.GetAllPagedAsync(filter);

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IFinalResult> AddAsync([FromBody] AddFloatingUnitDto dto) => await floatingUnitService.AddAsync(dto);

        /// <summary>
        /// Get All Data paged For Drop Down
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getDropDown")]
        public async Task<PagingResult> GetDropDownAsync([FromBody] BaseParam<SearchCriteriaFilter> filter) => await floatingUnitService.GetDropDownAsync(filter);

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IFinalResult> UpdateAsync(AddFloatingUnitDto model) => await floatingUnitService.UpdateAsync(model);

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IFinalResult> DeleteAsync(Guid id) => await floatingUnitService.DeleteAsync(id);

        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<IFinalResult> DeleteSoftAsync(Guid id) => await floatingUnitService.DeleteSoftAsync(id);
    }
}
