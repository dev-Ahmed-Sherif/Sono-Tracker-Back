using SonoTracker.Common.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization
{
    [ExcludeFromCodeCoverage]

    public class EditFloatingUnitOrganizationDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid FloatingUnitId { get; set; }

    }
}
