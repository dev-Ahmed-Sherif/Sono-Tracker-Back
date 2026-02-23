using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.GovernorateGeoPoint.Parameters;
using SonoTracker.Common.DTO.Tracker.GovernorateGeoPoint;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.GovernorateGeoPoint
{
    public interface IGovernorateGeoPointService : IBaseService<Domain.Entities.Tracker.GovernorateGeoPoint, AddGovernorateGeoPointDto, EditGovernorateGeoPointDto, GovernorateGeoPointDto, Guid, Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<GovernorateGeoPointFilter> filter);

        Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids);
        
    }
}
