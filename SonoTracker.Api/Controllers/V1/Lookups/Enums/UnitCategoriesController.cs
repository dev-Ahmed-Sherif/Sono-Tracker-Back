using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Application.Services.Enums.UnitCategories;
using SonoTracker.Common.Core;

namespace SonoTracker.Api.Controllers.V1.Lookups.Enums
{
    /// <summary>
    /// Constructor
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitCategoriesController(IUnitCategoryService unitCategory) : ControllerBase
    {
        /// <summary>
        /// Get all 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await unitCategory.GetAllAsync();
    }
}
