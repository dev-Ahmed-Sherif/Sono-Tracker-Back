using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class MaintenanceType : Lookup<Guid>
    {
        public virtual ICollection<Maintenance> Maintenances { get; set; } = new HashSet<Maintenance>();
    }
}
