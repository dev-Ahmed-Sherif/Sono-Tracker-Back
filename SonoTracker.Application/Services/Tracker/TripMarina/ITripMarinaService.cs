using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.MarinaTrip;
using SonoTracker.Common.DTO.Tracker.MarinaTrip.Parameters;


namespace SonoTracker.Application.Services.Tracker.TripMarina
{
    public interface ITripMarinaService : IBaseService<Entities.Tracker.TripMarina, AddMarinaTripDto, EditMarinaTripDto, MarinaTripDto, string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<MarinaTripFilter> filter, CancellationToken cancellationToken = default);

        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}