using SonoTracker.Domain.Entities.Attachments;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class TripInformation : BaseAudit<string>
    {
        public TripInformation()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        public string Code { get; set; }
        public DateTime SartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StaffNumber { get; set; }
        public int PassengerNumber { get; set; }
        public string? PassengerAttachment { get; set; }

        [MaxLength(50), ForeignKey(nameof(FloatingUnit))]
        public required string FloatingUnitId { get; set; }
        public virtual FloatingUnit? FloatingUnit { get; set; }

        [MaxLength(50), ForeignKey(nameof(Route))]
        public required string RouteId { get; set; }
        public virtual Route? Route { get; set; }
        
        [MaxLength(50), ForeignKey(nameof(Governorate))]
        public string? GovernorateId { get; set; }
        public virtual Governorate? Governorate { get; set; }

        public virtual HashSet<NationalityTrip> NationalityTrips { get; set; } = [];
        public virtual HashSet<TripMarinas> TripMarinas { get; set; } = [];
        public virtual HashSet<Inspection> Inspections { get; set; } = [];
        public virtual HashSet<PassengerTripAttachment> PassengerTripAttachments { get; set; } = [];
    }
}
