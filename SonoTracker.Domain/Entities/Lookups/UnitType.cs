using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using SonoTracker.Domain.Enum;
using System;
using System.Collections.Generic;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class UnitType : Lookup<string>
    {
        public UnitType() 
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }
        public UnitCategory UnitCategory { get; set; }
        public virtual HashSet<FloatingUnit> FloatingUnits { get; set; } = [];
    }
}
