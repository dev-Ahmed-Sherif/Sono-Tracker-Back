using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Enums.OrganizationTypes;
using SonoTracker.Common.Core;

namespace SonoTracker.Api.Controllers.V1.Lookups.Enums
{
    /// <summary>
    /// Constructor
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationTypesController(IOrganizationTypeService organizationType) : BaseController
    {
        /// <summary>
        /// Get all 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await organizationType.GetAllAsync();
    }
}
