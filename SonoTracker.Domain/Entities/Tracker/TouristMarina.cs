using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class TouristMarina : BaseEntity<Guid>
    {
        [MaxLength(50)]
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid TownId { get; set; }
        public Town Town { get; set; }
        public string Url { get; set; }
        public float Length { get; set; }
        public string NorthSide { get; set; }
        public string SouthSide { get; set; }
        public Guid GeoPointId { get; set; }
        public virtual GeoPoint GeoPoint { get; set; }
        public string Note { get; set; }
        public virtual ICollection<MarinaOrganization> MarinaOwners { get; set; } = new HashSet<MarinaOrganization>();
        public virtual ICollection<MarinaTrip> MarinaTrips { get; set; } = new HashSet<MarinaTrip>();
    }
}
