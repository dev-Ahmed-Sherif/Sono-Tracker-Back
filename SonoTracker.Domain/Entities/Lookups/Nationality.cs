using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Lookups
{
    [ExcludeFromCodeCoverage]
    public class Nationality : Lookup<Guid>
    {
        public virtual ICollection<Organization> Organizations { get; set; } = new HashSet<Organization>();

        public virtual ICollection<OrganizationStaff> OrganizationStaffs { get; set; } = new HashSet<OrganizationStaff>();

        public virtual ICollection<FloatingUnitStaff> FloatingUnitStaffs { get; set; } = new HashSet<FloatingUnitStaff>();

        public virtual ICollection<NationalityTrip> NationalityTrips { get; set; } = new HashSet<NationalityTrip>();

    }
}
