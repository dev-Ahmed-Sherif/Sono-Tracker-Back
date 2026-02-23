using System;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.GeoPoint.Parameters;
using SonoTracker.Common.DTO.Lookup.GeoPoint;

namespace SonoTracker.Application.Services.LookUp.GeoPoint
{
    public interface IGeoPointService : IBaseService<Entities.Lookups.GeoPoint, AddGeoPointDto, EditGeoPointDto, GeoPointDto, Guid, Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<GeoPointFilter> filter);

        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);

    }
}
