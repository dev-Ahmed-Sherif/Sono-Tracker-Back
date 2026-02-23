using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class GeoPoint : Lookup<Guid>
    {
        [MaxLength(50)]
        public string North { get; set; }

        [MaxLength(50)]
        public string East { get; set; }

        public virtual ICollection<GovernorateGeoPoint> GovernorateGeoPoints { get; set; } = new HashSet<GovernorateGeoPoint>();

        public virtual ICollection<TouristMarina> TouristMarinas { get; set; } = new HashSet<TouristMarina>();
        public virtual ICollection<Accident> Accidents { get; set; } = new HashSet<Accident>();
    }
}
