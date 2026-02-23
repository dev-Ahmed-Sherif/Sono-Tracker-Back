using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.MaintenanceType;
using SonoTracker.Common.DTO.Lookup.MaintenanceType.Parameters;
using System;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Lookup.MaintenanceType
{
    public interface IMaintenanceTypeService : IBaseService<Entities.Lookups.MaintenanceType, AddMaintenanceTypeDto, EditMaintenanceTypeDto, MaintenanceTypeDto, Guid, Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<MaintenanceTypeFilter> filter);

        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);

    }
}
