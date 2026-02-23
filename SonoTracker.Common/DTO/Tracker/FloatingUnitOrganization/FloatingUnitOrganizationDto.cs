using SonoTracker.Common.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization
{
    [ExcludeFromCodeCoverage]

    public class FloatingUnitOrganizationDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }

        public Guid OrganizationId { get; set; }

        public string OrganizationNameAr { get; set; }

        public string OrganizationNameEn { get; set; }
        public EnumResult OrganizationType { get; set; }

        public Guid FloatingUnitId { get; set; }

        public string FloatingUnitNameAr { get; set; }

        public string FloatingUnitNameEn { get; set; }


    }
}
