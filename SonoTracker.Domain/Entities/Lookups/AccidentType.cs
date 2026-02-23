using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Lookups
{
    [ExcludeFromCodeCoverage]
    public class AccidentType : Lookup<Guid>
    {
        public virtual ICollection<Accident> Accidents { get; set; } = new HashSet<Accident>();
    }
}
