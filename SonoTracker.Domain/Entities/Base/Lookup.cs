using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Base
{
    [ExcludeFromCodeCoverage]
    public class Lookup<TKey> : BaseAudit<TKey>
    {
        [Required]
        public  required string Code { get; set; }
        [Required]
        public required string NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}
