using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class Governorate : BaseEntity<Guid>
    {
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? ImageUrl { get; set; } = string.Empty;
        public virtual ICollection<GovernorateGeoPoint> GovernorateGeoPoints { get; set; } = new HashSet<GovernorateGeoPoint>();
        public virtual ICollection<Town> Towns { get; set; } = new HashSet<Town>();

    }
}
