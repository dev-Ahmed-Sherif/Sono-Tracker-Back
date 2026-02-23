using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class NationalityTrip : BaseEntity<Guid>
    {
        public Guid NationalityId { get; set; }
        public Nationality Nationality { get; set; }
        public Guid TripInformationId { get; set; }
        public TripInformation TripInformation { get; set; }
        public int NationalityNumber { get; set; }
    }
}
