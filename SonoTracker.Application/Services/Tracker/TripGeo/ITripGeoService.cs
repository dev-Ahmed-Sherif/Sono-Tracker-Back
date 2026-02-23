using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripGeo;
using SonoTracker.Common.DTO.Tracker.TripGeo.Parameters;

namespace SonoTracker.Application.Services.Tracker.TripGeo
{
    public interface ITripGeoService : IBaseService<Entities.Tracker.TripGeo, AddTripGeoDto, EditTripGeoDto, TripGeoDto, Guid, Guid?>
    {
        Task<IFinalResult> GetLastByFloatingUnitIdAsync(object id);
        Task<PagingResult> GetAllPagedAsync(BaseParam<TripGeoFilter> filter);
        Task<IFinalResult> GetAllFilterAsync(TripGeoFilter filter);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);
    }
}
