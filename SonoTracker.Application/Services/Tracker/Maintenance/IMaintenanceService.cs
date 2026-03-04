using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.Maintenance;
using SonoTracker.Common.DTO.Tracker.Maintenance.Parameters;
using SonoTracker.Common.DTO.Tracker.MarinaOrganization;
using SonoTracker.Common.DTO.Tracker.MarinaOrganization.Parameters;
using SonoTracker.Common.DTO.Tracker.TouristMarina;
using SonoTracker.Common.DTO.Tracker.TouristMarina.Parameters;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.MarinaOrganization
{
   public  interface IMaintenanceService : IBaseService<Domain.Entities.Tracker.Maintenance, AddMaintenanceDto, EditMaintenanceDto, MaintenanceDto, string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<MaintenanceFilter> Filter);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);
    }
}
