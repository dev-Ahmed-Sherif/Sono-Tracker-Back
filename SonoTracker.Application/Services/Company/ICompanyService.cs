using System;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Company;
using SonoTracker.Common.DTO.Company.Parameters;

namespace SonoTracker.Application.Services.Test
{
    public interface ICompanyService : IBaseService<Entities.Company, AddCompanyDto , EditCompanyDto, CompanyDto, Guid, Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<CompanyFilter> filter);
    }
}
