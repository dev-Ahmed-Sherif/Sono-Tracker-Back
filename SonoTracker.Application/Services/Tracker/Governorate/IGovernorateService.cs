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
    public interface IGovernorateService : IBaseService<Domain.Entities.Tracker.Governorate, AddGovernorateDto, EditGovernorateDto, GovernorateDto, Guid, Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<GovernorateFilter> filter);

        Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);
    }
}
