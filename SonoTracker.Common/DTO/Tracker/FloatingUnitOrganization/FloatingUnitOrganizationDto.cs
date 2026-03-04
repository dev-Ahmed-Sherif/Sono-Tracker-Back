using SonoTracker.Common.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.FloatingUnitOrganization
{
    [ExcludeFromCodeCoverage]

    public class FloatingUnitOrganizationDto : IEntityDto<string>
    {
        public string? Id { get; set; }

        public string OrganizationId { get; set; }

        public string OrganizationNameAr { get; set; }

        public string OrganizationNameEn { get; set; }
        public EnumResult OrganizationType { get; set; }

        public string FloatingUnitId { get; set; }

        public string FloatingUnitNameAr { get; set; }

        public string FloatingUnitNameEn { get; set; }


    }
}
