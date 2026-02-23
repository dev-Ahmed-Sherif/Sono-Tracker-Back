using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class TripGeo : BaseEntity<Guid>
    {
        public Guid TripInformationId { get; set; }
        public virtual TripInformation TripInformation { get; set; }
        public Guid GeoPointId { get; set; }
        public virtual GeoPoint GeoPoint { get; set; }
    }
}
