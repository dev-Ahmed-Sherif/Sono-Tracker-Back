using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class GovernorateGeoPoint : BaseEntity<Guid>
    {
        public Guid GeoPointId { get; set; }
        public virtual GeoPoint GeoPoint { get; set; }
        public Guid GovernorateId { get; set; }
        public virtual Governorate Governorate { get; set; }
    }
}
