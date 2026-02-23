using System;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Domain.Enum;

namespace SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization.Parameters
{
    [ExcludeFromCodeCoverage]

    public class FloatingUnitOrganizationFilter
    {
        public Guid? OrganizationId { get; set; }
        public OrganizationType? OrganizationType { get; set; }

        public Guid? FloatingUnitId { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
