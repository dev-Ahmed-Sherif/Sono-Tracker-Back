using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.FloatingUnit.Parameters;
using SonoTracker.Common.DTO.Tracker.FloatingUnit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.FloatingUnit
{
    public interface IFloatingUnitService : IBaseService<Domain.Entities.Tracker.FloatingUnit, AddFloatingUnitDto, EditFloatingUnitDto, FloatingUnitDto, string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<FloatingUnitFilter> filter, CancellationToken cancellationToken = default);

        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids);

        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default);
    }
}
