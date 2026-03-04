using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.Governorate.Parameters;
using SonoTracker.Common.DTO.Tracker.Governorate;

namespace SonoTracker.Application.Services.Tracker.Governorate
{
    public interface IGovernorateService : IBaseService<Entities.Lookups.Governorate, AddGovernorateDto, EditGovernorateDto, GovernorateDto, string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<GovernorateFilter> filter);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);
    }
}
