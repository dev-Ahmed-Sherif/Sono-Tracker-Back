using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Lookup.AccidentType;
using SonoTracker.Common.DTO.Lookup.AccidentType.Parameters;

namespace SonoTracker.Application.Services.LookUp.AccidentType
{
    public interface IAccidentTypeService : IBaseService<Entities.Lookups.AccidentType, AddAccidentTypeDto, EditAccidentTypeDto, AccidentTypeDto,string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<AccidentTypeFilter> filter);

        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);

    }
}
