using System;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Domain.Entities.Base;

namespace SonoTracker.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class Company : BaseEntity<Guid>
    {
        public string NameEn { get; set; }

        public string NameAr { get; set; }

    }
}
