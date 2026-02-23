using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TouristMarina;
using SonoTracker.Common.DTO.Tracker.TouristMarina.Parameters;
using SonoTracker.Common.DTO.Tracker.TripInformation.Parameters;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.TouristMarina
{
   public  interface ITouristMarinaService : IBaseService<Domain.Entities.Tracker.TouristMarina, AddTouristMarinaDto,EditTouristMarinaDto, TouristMarinaDto, Guid,Guid?>
   {
        Task<PagingResult> GetAllPagedAsync(BaseParam<TouristMarinaFilter> Filter);
        Task<IFinalResult> GetAllFilterAsync(TouristMarinaFilter filter);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);
   }
}
