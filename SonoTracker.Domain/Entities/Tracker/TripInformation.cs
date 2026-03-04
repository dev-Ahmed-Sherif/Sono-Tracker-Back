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
    public class TripInformation : BaseEntity<string>
    {
        public TripInformation()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        public DateTime SartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Code { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(FloatingUnit))]
        public required string FloatingUnitId { get; set; }
        public virtual FloatingUnit? FloatingUnit { get; set; }
        public int StaffNumber { get; set; }
        public int PassengerNumber { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(Route))]
        public required string RouteId { get; set; }
        public virtual Route? Route { get; set; }
        public string? PassengerAttachment { get; set; }
        public virtual HashSet<NationalityTrip> NationalityTrips { get; set; } = [];
        public virtual HashSet<MarinaTrip> MarinaTrips { get; set; } = [];
        public virtual HashSet<Inspection> Inspections { get; set; } = [];
        public virtual HashSet<PassengerTripAttachment> PassengerTripAttachments { get; set; } = [];
    }
}
