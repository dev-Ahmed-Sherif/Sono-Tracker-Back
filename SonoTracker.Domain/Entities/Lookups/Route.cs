using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class Route : Lookup<Guid>
    {
        public virtual ICollection<TripInformation> TripInformations { get; set; } = new HashSet<TripInformation>();

    }
}
