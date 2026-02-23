using System;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Test;
using SonoTracker.Common.DTO.Test.Parameters;

namespace SonoTracker.Application.Services.Test
{
    public interface ITestService : IBaseService<Entities.Test, AddTestDto , EditTestDto, TestDto, Guid, Guid?>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<TestFilter> filter);
    }
}
