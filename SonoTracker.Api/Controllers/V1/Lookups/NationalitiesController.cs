using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Lookup.Nationality;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.Nationality;
using SonoTracker.Common.DTO.Lookup.Nationality.Parameters;
using SonoTracker.Domain.Entities.Lookups;
using System.Net;

namespace SonoTracker.Api.Controllers.V1.Lookups
{
    /// <summary>
    /// EntityTypes Controller
    /// </summary>
    /// <remarks>
    /// Nationality
    /// </remarks>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class NationalitiesController(INationalityService nationalityService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IFinalResult> GetAsync(Guid id) => await nationalityService.GetByIdAsync(id);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) => await nationalityService.GetByIdForEditAsync(id);

        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await nationalityService.GetAllAsync();
        
        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<NationalityFilter> filter) => await nationalityService.GetAllPagedAsync(filter);

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult<IFinalResult>> AddAsync([FromBody] AddNationalityDto dto)
        {
            IFinalResult res = await nationalityService.AddAsync(dto);

            if (res.Status == HttpStatusCode.BadRequest)
            {
                return BadRequest(res);
            }
            else if (res.Status == HttpStatusCode.Conflict)
            {
                return Conflict(res);
            }

            return Ok(res);
        }

        /// <summary>
        /// Get All Data paged For Drop Down
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getDropDown")]
        public async Task<PagingResult> GetDropDownAsync([FromBody] BaseParam<SearchCriteriaFilter> filter) => await nationalityService.GetDropDownAsync(filter);

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult<IFinalResult>> UpdateAsync([FromBody] AddNationalityDto model)
        {
            IFinalResult res = await nationalityService.UpdateAsync(model);

            if (res.Status == HttpStatusCode.BadRequest)
            {
                return BadRequest(res);
            }
            else if (res.Status == HttpStatusCode.Conflict)
            {
                return Conflict(res);
            }

            return Ok(res);
        }

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<IFinalResult>> DeleteAsync(Guid id)
        {
            IFinalResult res = await nationalityService.DeleteAsync(id);

            if (res.Status == HttpStatusCode.BadRequest)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<ActionResult<IFinalResult>> DeleteSoftAsync(Guid id)
        {
            IFinalResult res = await nationalityService.DeleteSoftAsync(id);

            if (res.Status == HttpStatusCode.BadRequest)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }
    }
}
