using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Common.Core;

namespace SonoTracker.Application.Services.Enums.InspectionTypes
{
    public interface IInspectionTypeService
    {
        Task<IFinalResult> GetAllAsync();
    }
}
