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
    public class TouristMarina : BaseAudit<string>
    {
        public TouristMarina()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [MaxLength(50)]
        public string Code { get; set; }
        public string Name { get; set; }

        [MaxLength(50), ForeignKey(nameof(Town))]
        public string? TownId { get; set; }
        public virtual Town? Town { get; set; }
        public string Url { get; set; }
        public float Length { get; set; }
        public string NorthSide { get; set; }
        public string SouthSide { get; set; }

        [MaxLength(50), ForeignKey(nameof(GeoPoint))]
        public string? GeoPointId { get; set; }
        public virtual GeoPoint? GeoPoint { get; set; }
        
        [MaxLength(50), ForeignKey(nameof(Governorate))]
        public string? GovernorateId { get; set; }
        public virtual Governorate? Governorate { get; set; }
        public string Note { get; set; }
        public virtual HashSet<MarinaOrganization> MarinaOwners { get; set; } = [];
        public virtual HashSet<TripMarinas> MarinaTrips { get; set; } = [];
    }
}
