using SonoTracker.Domain.Entities.Base;
using System;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class Category : Lookup<Guid>
    {
        public string Description { get; set; }
    }
}
