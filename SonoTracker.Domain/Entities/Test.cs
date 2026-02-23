using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Domain.Entities.Base;

namespace SonoTracker.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class Test : BaseEntity<Guid>
    {
        public string NameEn { get; set; }

        public string NameAr { get; set; }

        public virtual ICollection<TestAttachment> TestAttachments { get; set; } = new HashSet<TestAttachment>();
    }
}
