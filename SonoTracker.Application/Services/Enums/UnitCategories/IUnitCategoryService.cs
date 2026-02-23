using SonoTracker.Common.Core;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Enums.UnitCategories
{
    public interface IUnitCategoryService
    {
        Task<IFinalResult> GetAllAsync();
    }
}
