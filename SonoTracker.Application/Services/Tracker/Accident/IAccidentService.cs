using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.Accident;
using SonoTracker.Common.DTO.Tracker.Accident.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.Accident
{
   public  interface IAccidentService : IBaseService<Entities.Tracker.Accident, AddAccidentDto, EditAccidentDto, AccidentDto, Guid,Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<AccidentFilter> Filter);
        Task<IFinalResult> GetAllFilterAsync(AccidentFilter filter);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);
    }
}
