using Microsoft.AspNetCore.Mvc;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Api.Controllers.V1.Base;
using Asp.Versioning;
using SonoTracker.Common.DTO.Tracker.Accident.Parameters;
using SonoTracker.Common.DTO.Tracker.Accident;
using SonoTracker.Application.Services.Tracker.Accident;
using Microsoft.AspNetCore.Authorization;

namespace SonoTracker.Api.Controllers.V1.Tracker.Accident
{
    /// <summary>
    /// Constructor
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class AccidentController(IAccidentService accidentService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IFinalResult> GetAsync(Guid id) => await accidentService.GetByIdAsync(id);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) => await accidentService.GetByIdForEditAsync(id);

        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await accidentService.GetAllAsync();

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<AccidentFilter> filter) => await accidentService.GetAllPagedAsync(filter);

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IFinalResult> AddAsync([FromForm] AddAccidentDto dto) => await accidentService.AddAsync(dto);
       
        /// <summary>
        /// Get All Data paged For Drop Down
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getDropDown")]
        public async Task<PagingResult> GetDropDownAsync([FromBody] BaseParam<SearchCriteriaFilter> filter) => await accidentService.GetDropDownAsync(filter);

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IFinalResult> UpdateAsync([FromForm] AddAccidentDto model) => await accidentService.UpdateAsync(model);

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IFinalResult> DeleteAsync(Guid id) => await accidentService.DeleteAsync(id);

        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<IFinalResult> DeleteSoftAsync(Guid id) => await accidentService.DeleteSoftAsync(id);
    }
}
