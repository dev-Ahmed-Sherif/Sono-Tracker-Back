using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.UnitType;
using SonoTracker.Common.DTO.Lookup.UnitType.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.LookUp.UnitType
{
    public interface IUnitTypeService : IBaseService<Entities.Lookups.UnitType, AddUnitTypeDto, EditUnitTypeDto, UnitTypeDto,string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<UnitTypeFilter> Filter);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);
    }
}
