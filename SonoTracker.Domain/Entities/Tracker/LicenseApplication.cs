using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class LicenseApplication : BaseEntity<string>
    {
        public LicenseApplication()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [MaxLength(100)]
        public required string LicenseNumber { get; set; }
        public DateTime? LicenseDate { get; set; } = DateTime.Now;


        [Required]
        [MaxLength(50), ForeignKey(nameof(FromOrganization))]
        public required string FromOrganizationId { get; set; }
        public virtual Organization? FromOrganization { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(ToOrganization))]
        public required string ToOrganizationId { get; set; }
        public virtual Organization? ToOrganization { get; set; }

        public string? Text { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public string Insurance { get; set; }
        public string CommercialRegister { get; set; }
        public string Taxes { get; set; }
        public string CivilProtection { get; set; }
        public string Irrigation { get; set; }
        public string StateProperty { get; set; }
        public string? Other { get; set; }
        public int TouristMarinaNumber { get; set; }
        public bool SendMail { get; set; }
    }
}
