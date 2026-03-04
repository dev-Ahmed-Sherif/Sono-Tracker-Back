using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class GeoPoint : Lookup<string>
    {
        public GeoPoint()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [Required, MaxLength(50)]
        public required string North { get; set; }

        [Required, MaxLength(50)]
        public required string East { get; set; }

        public virtual HashSet<TouristMarina> TouristMarinas { get; set; } = [];
        public virtual HashSet<Accident> Accidents { get; set; } = [];
    }
}
