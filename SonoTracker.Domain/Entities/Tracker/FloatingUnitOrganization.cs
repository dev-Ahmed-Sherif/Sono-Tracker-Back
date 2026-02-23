using SonoTracker.Domain.Entities.Base;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class FloatingUnitOrganization : BaseEntity<Guid>
    {
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public Guid FloatingUnitId { get; set; }
        public virtual FloatingUnit FloatingUnit { get; set; }
    }
}
