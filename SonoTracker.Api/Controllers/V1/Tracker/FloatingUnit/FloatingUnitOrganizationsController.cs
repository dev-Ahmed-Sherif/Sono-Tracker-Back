using Microsoft.AspNetCore.Mvc;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Tracker.FloatingUnitOrganization;
using SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization.Parameters;
using SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization;
using Microsoft.AspNetCore.Authorization;

namespace SonoTracker.Api.Controllers.V1.Tracker.FloatingUnit
{
    /// <summary>
    /// Constructor
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FloatingUnitOrganizationsController(IFloatingUnitOrganizationService floatingUnitOrganizationService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IFinalResult> GetAsync(Guid id) => await floatingUnitOrganizationService.GetByIdAsync(id);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) => await floatingUnitOrganizationService.GetByIdForEditAsync(id);

        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await floatingUnitOrganizationService.GetAllAsync();
        
        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<FloatingUnitOrganizationFilter> filter) => await floatingUnitOrganizationService.GetAllPagedAsync(filter);

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IFinalResult> AddAsync([FromBody] AddFloatingUnitOrganizationDto dto) => await floatingUnitOrganizationService.AddAsync(dto);

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IFinalResult> UpdateAsync(AddFloatingUnitOrganizationDto model) => await floatingUnitOrganizationService.UpdateAsync(model);

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IFinalResult> DeleteAsync(Guid id) => await floatingUnitOrganizationService.DeleteAsync(id);

        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<IFinalResult> DeleteSoftAsync(Guid id) => await floatingUnitOrganizationService.DeleteSoftAsync(id);
    }
}
