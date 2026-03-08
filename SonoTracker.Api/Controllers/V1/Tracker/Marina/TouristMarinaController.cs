using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Lookup.Town;
using SonoTracker.Application.Services.Tracker.TouristMarina;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.Town;
using SonoTracker.Common.DTO.Tracker.TouristMarina;
using SonoTracker.Common.DTO.Tracker.TouristMarina.Parameters;
using System.Net;
using System.Threading;

namespace SonoTracker.Api.Controllers.V1.Tracker.Marina
{
    /// <summary>
    /// Constructor
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class TouristMarinaController(
        ITouristMarinaService touristMarinaService,
        ITownService townService) : BaseController
    {
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>

        [HttpGet("get/{id}")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        public async Task<IFinalResult> GetAsync(Guid id, CancellationToken cancellationToken = default)
                                        => await touristMarinaService.GetByIdAsync(id, cancellationToken);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>

        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id, CancellationToken cancellationToken = default)
                                        => await touristMarinaService.GetByIdForEditAsync(id, cancellationToken);

        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>

        [HttpGet("getAll")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IFinalResult>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IFinalResult res = await touristMarinaService.GetAllAsync(cancellationToken: cancellationToken);

            if (res.Status == HttpStatusCode.NotFound) return NotFound(res);

            return Ok(res);
        }

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagingResult>> GetPagedAsync([FromBody] BaseParam<TouristMarinaFilter> filter, CancellationToken cancellationToken = default)
        {
            PagingResult res = await touristMarinaService.GetAllPagedAsync(filter, cancellationToken);

            if (res.Status == HttpStatusCode.NotFound) return NotFound(res);

            return Ok(res);
        }

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IFinalResult> AddAsync([FromBody] AddTouristMarinaDto dto, CancellationToken cancellationToken = default)
        {
            string gov = "28", townCode = "";
            var town = await townService.GetByIdAsync(dto.TownId, cancellationToken);
            if (town.Data is TownDto townDto)
            {
                townCode = townDto.Code;

                TouristMarinaFilter filter = new()
                {
                    TownId = dto.TownId,
                    IsDeleted = false
                };

                var exist = await touristMarinaService.GetAllFilterAsync(filter);
                var existDataCollection = exist.Data as ICollection<TouristMarinaDto>;

                if (existDataCollection?.Count > 0)
                {
                    var lastItem = existDataCollection.OrderByDescending(x => x.Code).FirstOrDefault();

                    if (int.TryParse(lastItem!.Code.AsSpan(4, 3), out int number))
                    {
                        number++;
                        dto.Code = gov + townCode + number.ToString("D3");
                    }
                }
                else
                {
                    dto.Code = $"{gov}{townCode}001";
                }
            }

            return await touristMarinaService.AddAsync(dto, cancellationToken);
        } 

        /// <summary>
        /// Get All Data paged For Drop Down
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getDropDown")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagingResult>> GetDropDownAsync([FromBody] BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            PagingResult res = await touristMarinaService.GetDropDownAsync(filter, cancellationToken);

            if (res.Status == HttpStatusCode.NotFound) return NotFound(res);

            return Ok(res);
        }

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IFinalResult> UpdateAsync([FromBody] AddTouristMarinaDto model, CancellationToken cancellationToken = default)
                                        => await touristMarinaService.UpdateAsync(model, cancellationToken);

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IFinalResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
                                        => await touristMarinaService.DeleteAsync(id, cancellationToken);

        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<IFinalResult> DeleteSoftAsync(Guid id, CancellationToken cancellationToken = default)
                                        => await touristMarinaService.DeleteSoftAsync(id, cancellationToken);
    }
}
