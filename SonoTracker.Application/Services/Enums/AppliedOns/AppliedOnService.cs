using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Enums.AppliedOns
{
    public class AppliedOnService : IAppliedOnService
    {
        public async Task<IFinalResult> GetAllAsync()
        {
            var enumNames = Enum.GetValues(typeof(AppliedOn)).Cast<AppliedOn>().Select(e => e.GetName()).ToList();
            return await Task.FromResult(new ResponseResult().PostResult(enumNames, HttpStatusCode.OK));

        }
    }
}
