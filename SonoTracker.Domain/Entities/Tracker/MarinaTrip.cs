using SonoTracker.Domain.Entities.Base;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class MarinaTrip : BaseEntity<Guid>
    {
        public Guid TouristMarinaId { get; set; }
        public TouristMarina TouristMarina { get; set; }
        public Guid TripInformationId { get; set; }
        public TripInformation TripInformation { get; set; }
    }
}
