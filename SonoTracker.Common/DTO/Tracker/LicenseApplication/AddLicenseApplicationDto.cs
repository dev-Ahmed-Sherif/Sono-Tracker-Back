using Microsoft.AspNetCore.Http;
using SonoTracker.Common.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.LicenseApplication
{
    [ExcludeFromCodeCoverage]
    public class AddLicenseApplicationDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public string LicenseNumber { get; set; }
        public required IFormFile Insurance { get; set; }
        public required IFormFile CommercialRegister { get; set; }
        public required IFormFile Taxes { get; set; }
        public required IFormFile CivilProtection { get; set; }
        public required IFormFile Irrigation { get; set; }
        public required IFormFile StateProperty { get; set; }
        public IFormFile Other { get; set; }
        public required Guid FromOrganizationId { get; set; }
        public required Guid ToOrganizationId { get; set; }
        public int TouristMarinaNumber { get; set; }
        public bool SendMail { get; set; }
    }
}
