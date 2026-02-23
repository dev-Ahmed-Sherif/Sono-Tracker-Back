using SonoTracker.Domain.Entities.Base;
using System;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class Category : Lookup<Guid>
    {
        public string NameEn { get; set; }

        public string NameAr { get; set; }

        public string Description { get; set; }
    }
}
