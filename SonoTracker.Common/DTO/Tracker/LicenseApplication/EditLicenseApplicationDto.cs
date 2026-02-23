using Microsoft.AspNetCore.Http;
using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.LicenseApplication
{
    [ExcludeFromCodeCoverage]
    public class EditLicenseApplicationDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public int LicenseNumber { get; set; }
        public string Insurance { get; set; }
        public string CommercialRegister { get; set; }
        public string Taxes { get; set; }
        public string CivilProtection { get; set; }
        public string Irrigation { get; set; }
        public string StateProperty { get; set; }
        public string Other { get; set; }
        public int TouristMarinaNumber { get; set; }
        public bool SendMail { get; set; }
    }
}
