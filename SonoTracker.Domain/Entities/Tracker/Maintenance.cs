using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class Maintenance : BaseEntity<string>
    {
        public Maintenance()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        public string Number { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(MaintenanceType))]
        public required string MaintenanceTypeId { get; set; }
        public virtual MaintenanceType? MaintenanceType { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(FloatingUnit))]
        public required string FloatingUnitId { get; set; }
        public virtual FloatingUnit? FloatingUnit { get; set; }
        public string MaintenanceReport { get; set; }
        public string? Other{ get; set; }
        public string? Notes { get; set; }
    }
}
