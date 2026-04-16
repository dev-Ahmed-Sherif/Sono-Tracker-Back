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
        public string Number { get; set; }
        public DateOnly MaintenanceDate { get; set; }
        public DateOnly? NextMaintenanceDate { get; set; }
        public string MaintenanceTypeId { get; set; }
        public string FloatingUnitId { get; set; }
        public IFormFile MaintenanceReport { get; set; }
        public IFormFile? Other { get; set; }
        public string? Notes { get; set; }
       
    }
}
