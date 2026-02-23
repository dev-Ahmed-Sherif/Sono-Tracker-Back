using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Test;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Test;
using SonoTracker.Common.DTO.Test.Parameters;

namespace SonoTracker.Api.Controllers.V2.Business
{
    /// <summary>
    /// Test Controller
    /// </summary>
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestsController : BaseController
    {
        private readonly ITestService _testService;
        /// <summary>
        /// Constructor
        /// </summary>
        public TestsController(ITestService testService)
        {
            _testService = testService;
        }


        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IFinalResult> GetAsync(Guid id) => await _testService.GetByIdAsync(id);


        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) => await _testService.GetByIdForEditAsync(id);


        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync()
        {
            return await _testService.GetAllAsync();
        }

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<TestFilter> filter) => await _testService.GetAllPagedAsync(filter);


        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IFinalResult> AddAsync([FromBody] AddTestDto dto) => await _testService.AddAsync(dto);



        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IFinalResult> UpdateAsync(AddTestDto model) => await _testService.UpdateAsync(model);

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IFinalResult> DeleteAsync(Guid id) => await _testService.DeleteAsync(id);


        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<IFinalResult> DeleteSoftAsync(Guid id) => await _testService.DeleteSoftAsync(id);


    }
}
