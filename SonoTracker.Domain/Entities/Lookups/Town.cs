using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class Town : Lookup<Guid>
    {
        public Guid CityId { get; set; }

        public virtual City City { get; set; }

        public Guid GovernorateId { get; set; }

        public virtual Governorate Governorate { get; set; }

        public virtual ICollection<TouristMarina> TouristMarinas { get; set; } = new HashSet<TouristMarina>();
        public virtual ICollection<Accident> Accidents { get; set; } = new HashSet<Accident>();


    }
}
