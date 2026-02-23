using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.InspectionType.Parameters;
using SonoTracker.Common.DTO.Lookup.InspectionType;

namespace SonoTracker.Application.Services.LookUp.InspectionType
{
    public interface IInspectionTypeService : IBaseService<Entities.Lookups.InspectionType, AddInspectionTypeDto, EditInspectionTypeDto, InspectionTypeDto, Guid, Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<InspectionTypeFilter> filter);

        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);

        //  Task<IFinalResult> DeleteBulkAsync(IEnumerable<int> ids);
    }
}
