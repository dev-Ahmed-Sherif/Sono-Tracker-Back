using System;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Base;

namespace SonoTracker.Common.DTO.Lookup.Category
{
    [ExcludeFromCodeCoverage]
    public class CategoryDto : LookupDto<int?>
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
