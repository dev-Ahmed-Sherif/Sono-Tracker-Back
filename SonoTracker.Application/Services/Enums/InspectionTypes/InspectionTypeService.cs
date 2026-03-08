using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Enums.OrganizationTypes;
using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;

namespace SonoTracker.Application.Services.Enums.InspectionTypes
{
    public class InspectionTypeService : IInspectionTypeService
    {
        public async Task<IFinalResult> GetAllAsync()
        {
            var enumNames = Enum.GetValues(typeof(InspectionType)).Cast<InspectionType>().Select(e => e.GetName()).ToList();
            return await Task.FromResult(new ResponseResult().PostResult(enumNames, HttpStatusCode.OK));

        }
    }
}
