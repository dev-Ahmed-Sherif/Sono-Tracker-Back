using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.Governorate.Parameters;
using SonoTracker.Common.DTO.Tracker.Governorate;
using SonoTracker.Common.DTO.Tracker.LicenseApplication;
using SonoTracker.Common.DTO.Tracker.LicenseApplication.Parameters;

namespace SonoTracker.Application.Services.Tracker.LicenseApplication
{
    public interface ILicenseApplicationService : IBaseService<Entities.Tracker.LicenseApplication, AddLicenseApplicationDto, EditLicenseApplicationDto, LicenseApplicationDto, string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<LicenseApplicationFilter> filter, CancellationToken cancellationToken = default);
        Task<IFinalResult> GetAllFilterAsync(LicenseApplicationFilter filter);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default);
    }
}
