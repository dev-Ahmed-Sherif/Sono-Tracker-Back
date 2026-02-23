using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.FloatingUnitStaff;
using SonoTracker.Common.DTO.Tracker.FloatingUnitStaff.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.FloatingUnitStaff
{
    public interface IFloatingUnitStaffService : IBaseService<Domain.Entities.Tracker.FloatingUnitStaff, AddFloatingUnitStaffDto, EditFloatingUnitStaffDto, FloatingUnitStaffDto, Guid, Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<FloatingUnitStaffFilter> filter);

        Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids);
        
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);

    }
}
