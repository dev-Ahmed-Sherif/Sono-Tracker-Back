using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.Accident
{
    [ExcludeFromCodeCoverage]
    public class EditAccidentDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public string Number { get; set; }
        public string Town { get; set; }
        public Guid GeoPointId { get; set; }
        public DateTime AccidentDate { get; set; }
        public DateTime ResponseDate { get; set; }
        public string AccidentType { get; set; }
        public Guid AccidentTypeId { get; set; }
        public string FloatingUnit { get; set; }
        public Guid FloatingUnitId { get; set; }
        public Case CaseId { get; set; }
        public EnumResult Case { get; set; }
        public string Organization { get; set; }
        public Guid OrganizationId { get; set; }
        public string Notes { get; set; }
        public string Attach { get; set; }
    }
}
