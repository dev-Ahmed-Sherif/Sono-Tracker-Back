using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripNationality;
using SonoTracker.Common.DTO.Tracker.TripNationality.Parameters;

namespace SonoTracker.Application.Services.Tracker.TripNationality
{
    public interface ITripNationalityService : IBaseService<Entities.Tracker.TripNationality, AddTripNationalityDto, EditTripNationalityDto, TripNationalityDto, string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<TripNationalityFilter> filter, CancellationToken cancellationToken = default);
        Task<IFinalResult> GetAllFilterAsync(TripNationalityFilter filter, CancellationToken cancellationToken = default);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default);
    }
}
