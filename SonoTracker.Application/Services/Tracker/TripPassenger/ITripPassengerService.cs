using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripPassenger;
using SonoTracker.Common.DTO.Tracker.TripPassenger.Parameters;

namespace SonoTracker.Application.Services.Tracker.TripPassenger
{
    public interface ITripPassengerService : IBaseService<Entities.Tracker.TripPassenger, AddTripPassengerDto, EditTripPassengerDto, TripPassengerDto, string, string>
    {
        Task<IFinalResult> GetAllAsync(string? tripInformationId, CancellationToken cancellationToken = default);
        Task<PagingResult> GetAllPagedAsync(BaseParam<TripPassengerFilter> filter, CancellationToken cancellationToken = default);
        Task<IFinalResult> GetAllFilterAsync(TripPassengerFilter filter, CancellationToken cancellationToken = default);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default);
    }
}
