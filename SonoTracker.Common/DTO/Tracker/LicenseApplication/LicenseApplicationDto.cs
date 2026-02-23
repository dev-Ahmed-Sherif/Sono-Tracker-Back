using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.LicenseApplication
{
    [ExcludeFromCodeCoverage]

    public class LicenseApplicationDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime? LicenseDate { get; set; }
        public Guid FromOrganizationId { get; set; }
        public string FromOrganizationNameAr { get; set; }
        public Guid ToOrganizationId { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public string Insurance { get; set; }
        public string CommercialRegister { get; set; }
        public string Taxes { get; set; }
        public string CivilProtection { get; set; }
        public string Irrigation { get; set; }
        public string StateProperty { get; set; }
        public string Other { get; set; }
        public int TouristMarinaNumber { get; set; }
    }
}
