using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.Town;
using SonoTracker.Common.DTO.Lookup.Town.Parameters;
using System;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Lookup.Town
{
    public interface ITownService : IBaseService<Domain.Entities.Lookups.Town, AddTownDto, EditTownDto, TownDto, string, string>
    {
    Task<PagingResult> GetAllPagedAsync(BaseParam<TownFilter> filter);

    Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);
    }
}
