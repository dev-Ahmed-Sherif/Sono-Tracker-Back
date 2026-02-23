using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
   [ExcludeFromCodeCoverage]
   public class Accident : BaseEntity<Guid>
   {
        public string Number { get; set; }
        public Guid TownId { get; set; }
        public virtual Town Town { get; set; }
        public Guid GeoPointId { get; set; }   
        public virtual GeoPoint GeoPoint { get; set; }
        public DateTime AccidentDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public Guid AccidentTypeId { get; set; }
        public virtual AccidentType AccidentType { get; set; }
        public Guid FloatingUnitId { get; set; }
        public virtual FloatingUnit FloatingUnit { get; set; }
        public  Case CaseId { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public string? Notes { get; set; }
        public string? Attach { get; set; }
   }
}
