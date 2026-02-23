using SonoTracker.Common.Core;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Enums.AppliedOns
{
    public interface IAppliedOnService
    {
        Task<IFinalResult> GetAllAsync();
    }
}
