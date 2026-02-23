using Microsoft.AspNetCore.Http;
using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.Accident
{
    [ExcludeFromCodeCoverage]
    public class AddAccidentDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public string Number { get; set; }
        public Guid TownId { get; set; }
        public Guid GeoPointId { get; set; }
        public DateTime AccidentDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public Guid AccidentTypeId { get; set; }
        public Guid FloatingUnitId { get; set; }
        public Case CaseId { get; set; }
        public Guid OrganizationId { get; set; }
        public string? Notes { get; set; }
        public IFormFile? Attach { get; set; }
      
       
    }
}
