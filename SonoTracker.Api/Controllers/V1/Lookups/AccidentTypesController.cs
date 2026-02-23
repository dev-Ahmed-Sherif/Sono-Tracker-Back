using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.LookUp.AccidentType;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.AccidentType;
using SonoTracker.Common.DTO.Lookup.AccidentType.Parameters;
using System.Net;

namespace SonoTracker.Api.Controllers.V1.Lookups
{
    /// <summary>
    /// EntityTypes Controller
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class AccidentTypesController(IAccidentTypeService accidentTypeService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IFinalResult> GetAsync(Guid id) => await accidentTypeService.GetByIdAsync(id);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) => await accidentTypeService.GetByIdForEditAsync(id);

        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await accidentTypeService.GetAllAsync();
        
        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<AccidentTypeFilter> filter) => await accidentTypeService.GetAllPagedAsync(filter);


        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult<IFinalResult>> AddAsync([FromBody] AddAccidentTypeDto dto) 
        { 
            IFinalResult res = await accidentTypeService.AddAsync(dto);

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
        public async Task<PagingResult> GetDropDownAsync([FromBody] BaseParam<SearchCriteriaFilter> filter) => await accidentTypeService.GetDropDownAsync(filter);

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult<IFinalResult>> UpdateAsync([FromBody] AddAccidentTypeDto model)
        {
            IFinalResult res = await accidentTypeService.UpdateAsync(model);

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
            IFinalResult res = await accidentTypeService.DeleteAsync(id);

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
            IFinalResult res = await accidentTypeService.DeleteSoftAsync(id);

            if (res.Status == HttpStatusCode.BadRequest)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }
    }
}
