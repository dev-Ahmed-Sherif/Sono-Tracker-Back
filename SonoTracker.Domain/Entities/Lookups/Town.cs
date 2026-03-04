using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class Town : Lookup<string>
    {
        public Town()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [Required]
        [MaxLength(50), ForeignKey(nameof(City))]
        public required string CityId { get; set; }
        public virtual City? City { get; set; }

        public virtual HashSet<Accident> Accidents { get; set; } = [];
        public virtual HashSet<TouristMarina> TouristMarinas { get; set; } = [];

    }
}
