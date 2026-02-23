using SonoTracker.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class City : Lookup<Guid>
    {
        public virtual ICollection<Town> Towns { get; set; } = new HashSet<Town>();
    }
}
