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
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public float Length { get; set; }
        public string NorthSide { get; set; } = string.Empty;
        public string SouthSide { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        [ForeignKey(nameof(City))]
        public required string CityId { get; set; }
        public virtual City? City { get; set; }

        [MaxLength(50), ForeignKey(nameof(GeoPoint))]
        public string? GeoPointId { get; set; }
        public virtual GeoPoint? GeoPoint { get; set; }
        
        [MaxLength(50), ForeignKey(nameof(Governorate))]
        public string? GovernorateId { get; set; }
        public virtual Governorate? Governorate { get; set; }
        public virtual HashSet<TouristMarinaOrganization> TouristMarinaOwners { get; set; } = [];
        public virtual HashSet<TripMarina> MarinaTrips { get; set; } = [];
    }
}
