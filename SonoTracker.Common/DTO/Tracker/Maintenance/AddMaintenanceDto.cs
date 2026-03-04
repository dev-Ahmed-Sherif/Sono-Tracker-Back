using Microsoft.AspNetCore.Http;
using SonoTracker.Common.Core;
using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.Maintenance
{
    [ExcludeFromCodeCoverage]

    public class AddMaintenanceDto : IEntityDto<string>
    {
        public string? Id { get; set; }
        public int Number { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string MaintenanceTypeId { get; set; }
        public string FloatingUnitId { get; set; }
        public IFormFile MaintenanceReport { get; set; }
        public IFormFile? Other { get; set; }
        public string? Notes { get; set; }
       
    }
}
