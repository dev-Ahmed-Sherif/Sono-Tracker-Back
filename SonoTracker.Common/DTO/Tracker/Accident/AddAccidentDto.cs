using Microsoft.AspNetCore.Http;
using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.Accident
{
    [ExcludeFromCodeCoverage]
    public class AddAccidentDto : IEntityDto<string>
    {
        public string? Id { get; set; }
        public string Number { get; set; }
        public string TownId { get; set; }
        public string GeoPointId { get; set; }
        public DateTime AccidentDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string AccidentTypeId { get; set; }
        public string FloatingUnitId { get; set; }
        public Case CaseId { get; set; }
        public string OrganizationId { get; set; }
        public string? Notes { get; set; }
        public IFormFile? Attach { get; set; }
      
       
    }
}
