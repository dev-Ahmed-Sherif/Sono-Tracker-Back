using Microsoft.AspNetCore.Mvc;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Application.Services.Tracker.TouristMarina;
using SonoTracker.Common.DTO.Tracker.TouristMarina;
using SonoTracker.Common.DTO.Tracker.TouristMarina.Parameters;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Common.DTO.Lookup.Town;
using SonoTracker.Application.Services.Lookup.Town;
using Microsoft.AspNetCore.Authorization;

namespace SonoTracker.Api.Controllers.V1.Tracker.Marina
{
    /// <summary>
    /// Constructor
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IFinalResult> GetAsync(Guid id) => await touristMarinaService.GetByIdAsync(id);

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        public async Task<IFinalResult> GetEditAsync(Guid id) => await touristMarinaService.GetByIdForEditAsync(id);


        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IFinalResult> GetAllAsync() => await touristMarinaService.GetAllAsync();

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        public async Task<PagingResult> GetPagedAsync([FromBody] BaseParam<TouristMarinaFilter> filter) => await touristMarinaService.GetAllPagedAsync(filter);

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IFinalResult> AddAsync([FromBody] AddTouristMarinaDto dto) 
        {
            string gov = "28" , townCode;
            var town = await townService.GetByIdAsync(dto.TownId);
 ;
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
            
            var res = await touristMarinaService.AddAsync(dto);

            return res;
        } 

        /// <summary>
        /// Get All Data paged For Drop Down
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getDropDown")]
        public async Task<PagingResult> GetDropDownAsync([FromBody] BaseParam<SearchCriteriaFilter> filter) => await touristMarinaService.GetDropDownAsync(filter);

        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IFinalResult> UpdateAsync(AddTouristMarinaDto model) => await touristMarinaService.UpdateAsync(model);

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IFinalResult> DeleteAsync(Guid id) => await touristMarinaService.DeleteAsync(id);

        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        public async Task<IFinalResult> DeleteSoftAsync(Guid id) => await touristMarinaService.DeleteSoftAsync(id);
    }
}
