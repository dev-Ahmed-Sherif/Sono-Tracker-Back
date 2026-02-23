using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Base;

namespace SonoTracker.Common.DTO.Lookup.Status
{
    [ExcludeFromCodeCoverage]
    public class AddStatusDto : LookupDto<int?>
    {
        public string EntityName { get; set; }

        public string CssClass { get; set; }
    }
}
