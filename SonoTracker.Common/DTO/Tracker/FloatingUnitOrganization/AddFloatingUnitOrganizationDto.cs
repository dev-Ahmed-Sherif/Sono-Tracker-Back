using SonoTracker.Common.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization
{
    [ExcludeFromCodeCoverage]

    public class AddFloatingUnitOrganizationDto : IEntityDto<string>
    {
        public string? Id { get; set; }

        public string OrganizationId { get; set; }

        public string FloatingUnitId { get; set; }

    }
}
