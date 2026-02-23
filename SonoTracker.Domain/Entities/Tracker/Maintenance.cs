using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class Maintenance : BaseEntity<Guid>
    {
        public string Number { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public Guid MaintenanceTypeId { get; set; }
        public virtual MaintenanceType MaintenanceType { get; set; }
        public Guid FloatingUnitId { get; set; }
        public virtual FloatingUnit FloatingUnit { get; set; }
        public string MaintenanceReport { get; set; }
        public string? Other{ get; set; }
        public string? Notes { get; set; }
    }
}
