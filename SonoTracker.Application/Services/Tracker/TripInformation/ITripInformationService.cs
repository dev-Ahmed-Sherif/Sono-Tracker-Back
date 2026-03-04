using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripInformation.Parameters;
using SonoTracker.Common.DTO.Tracker.TripInformation;

namespace SonoTracker.Application.Services.Tracker.TripInformation
{
    public interface ITripInformationService : IBaseService<Domain.Entities.Tracker.TripInformation, AddTripInformationDto, EditTripInformationDto, TripInformationDto, string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<TripInformationFilter> filter);
        Task<IFinalResult> GetAllFilterAsync(TripInformationFilter filter);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);
    }
}
