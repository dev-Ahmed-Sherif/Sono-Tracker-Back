using SonoTracker.Domain.Entities.Base;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Lookups
{
    [ExcludeFromCodeCoverage]
    public class Status : Lookup<int>
    {
        public string EntityName { get; set; }

        public string CssClass { get; set; }
    }
}
