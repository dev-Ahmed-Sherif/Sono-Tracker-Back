using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class TripGeo : BaseEntity<string>
    {
        public TripGeo()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [Required, MaxLength(50)] 
        [ForeignKey(nameof(TripInformation))]
        public required string TripInformationId { get; set; }
        public virtual TripInformation? TripInformation { get; set; }

        [Required, MaxLength(50)] 
        [ForeignKey(nameof(GeoPoint))]
        public required string GeoPointId { get; set; }
        public virtual GeoPoint? GeoPoint { get; set; }
    }
}
