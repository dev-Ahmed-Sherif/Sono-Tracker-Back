using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization;
using SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization.Parameters;

namespace SonoTracker.Application.Services.Tracker.FloatingUnitOrganization
{
    public interface IFloatingUnitOrganizationService : IBaseService<Domain.Entities.Tracker.FloatingUnitOrganization, AddFloatingUnitOrganizationDto, EditFloatingUnitOrganizationDto, FloatingUnitOrganizationDto, string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<FloatingUnitOrganizationFilter> filter);

        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids);

    }
}
