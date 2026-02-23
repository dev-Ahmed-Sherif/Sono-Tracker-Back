using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Base;

namespace SonoTracker.Common.DTO.Lookup.Category
{
    [ExcludeFromCodeCoverage]
    public class AddCategoryDto : LookupDto<int?>
    {
        public string Description { get; set; }
    }
}
