using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.Inspection;
using SonoTracker.Common.DTO.Tracker.GeneralInspection.Parameters;
using SonoTracker.Common.DTO.Tracker.GeneralInspection;

namespace SonoTracker.Application.Services.Tracker.GeneralInspection
{
    public interface IGeneralInspectionService : IBaseService<Entities.Tracker.Inspection, AddGeneralInspectionDto, EditGeneralInspectionDto, GeneralInspectionDto, Guid, Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<GeneralInspectionFilter> filter);

        Task<IFinalResult> DeleteRangeAsync(IEnumerable<Guid> ids);
       
    }
}
