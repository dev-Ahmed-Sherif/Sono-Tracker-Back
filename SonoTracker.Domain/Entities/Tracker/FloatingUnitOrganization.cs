using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class FloatingUnitOrganization : BaseEntity<string>
    {
        public FloatingUnitOrganization()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [MaxLength(50), ForeignKey(nameof(Organization))]
        public string? OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }

        [MaxLength(50), ForeignKey(nameof(FloatingUnit))]
        public string? FloatingUnitId { get; set; }
        public virtual FloatingUnit? FloatingUnit { get; set; }
    }
}
