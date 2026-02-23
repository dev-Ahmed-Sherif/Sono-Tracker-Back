using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Base;

namespace SonoTracker.Common.DTO.Lookup.Category
{
    [ExcludeFromCodeCoverage]
    public class EditCategoryDto : LookupDto<int?>
    {
        public string Description { get; set; }
    }
}
