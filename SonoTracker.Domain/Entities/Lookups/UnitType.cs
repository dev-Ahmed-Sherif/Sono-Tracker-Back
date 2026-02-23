using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class UnitType : Lookup<Guid>
    {
        public virtual ICollection<FloatingUnit> FloatingUnits { get; set; } = new HashSet<FloatingUnit>();

    }
}
