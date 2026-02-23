using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Reports.Org;
using SonoTracker.Common.DTO.Tracker.Organization;
using SonoTracker.Common.DTO.Tracker.Organization.Parameters;

namespace SonoTracker.Application.Services.Tracker.Organization
{
    public interface IOrganizationService : IBaseService<Domain.Entities.Tracker.Organization, AddOrganizationDto, EditOrganizationDto, OrganizationDto, Guid, Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<OrganizationFilter> filter);

        Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids);

        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);

        Task<IFinalResult> GetAllReportAsync(FilterOrgReportDTO filter);
        Task<byte[]> GenerateReportAsync(FilterOrgReportDTO filter, CancellationToken cancellationToken = default);
    }
}
