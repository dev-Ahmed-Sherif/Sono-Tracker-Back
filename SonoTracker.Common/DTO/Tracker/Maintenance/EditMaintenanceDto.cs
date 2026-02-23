using SonoTracker.Common.Core;
using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.Maintenance
{
    [ExcludeFromCodeCoverage]
    public class EditMaintenanceDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public string Number { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public Guid MaintenanceTypeId { get; set; }
        public string MaintenanceTypeNameAr { get; set; }
        public Guid FloatingUnitId { get; set; }
        public string FloatingUnitNameAr { get; set; }
        public string MaintenanceReport { get; set; }
        public string? Other { get; set; }
        public string? Notes { get; set; }
    }
}
