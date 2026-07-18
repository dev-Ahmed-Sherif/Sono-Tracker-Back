using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripStaff;
using SonoTracker.Common.DTO.Tracker.TripStaff.Parameters;

namespace SonoTracker.Application.Services.Tracker.TripStaff
{
    public interface ITripStaffService : IBaseService<Entities.Tracker.TripStaff, AddTripStaffDto, EditTripStaffDto, TripStaffDto, string, string>
    {
        Task<IFinalResult> GetAllAsync(string? tripInformationId, CancellationToken cancellationToken = default);
        Task<PagingResult> GetAllPagedAsync(BaseParam<TripStaffFilter> filter, CancellationToken cancellationToken = default);
        Task<IFinalResult> GetAllFilterAsync(TripStaffFilter filter, CancellationToken cancellationToken = default);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default);
    }
}
