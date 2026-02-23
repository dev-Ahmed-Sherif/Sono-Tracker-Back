using System;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Base;

namespace SonoTracker.Common.DTO.Lookup.Action
{
    [ExcludeFromCodeCoverage]
    public class ActionDto : LookupDto<int?>
    {
        public DateTime CreatedDate { get; set; }
    }
}
