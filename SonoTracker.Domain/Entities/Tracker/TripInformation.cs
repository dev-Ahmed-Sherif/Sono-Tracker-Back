using SonoTracker.Domain.Entities.Attachments;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class TripInformation : BaseEntity<Guid>
    {
        public DateTime SartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Code { get; set; }
        public Guid FloatingUnitId { get; set; }
        public virtual FloatingUnit FloatingUnit { get; set; }
        public int StaffNumber { get; set; }
        public int PassengerNumber { get; set; }
        public Guid RouteId { get; set; }
        public virtual Route Route { get; set; }
        public string? PassengerAttachment { get; set; }
        public virtual ICollection<NationalityTrip> NationalityTrips { get; set; } = new HashSet<NationalityTrip>();
        public virtual ICollection<MarinaTrip> MarinaTrips { get; set; } = new HashSet<MarinaTrip>();
        public virtual ICollection<Inspection> Inspections { get; set; } = new HashSet<Inspection>();
        public virtual ICollection<PassengerTripAttachment> PassengerTripAttachments { get; set; } = new HashSet<PassengerTripAttachment>();
    }
}
