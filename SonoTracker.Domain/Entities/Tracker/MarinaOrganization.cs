using SonoTracker.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class MarinaOrganization : BaseEntity<string>
    {
        public MarinaOrganization()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [MaxLength(100)]
        public string LicenseNumber { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(TouristMarina))]
        public required string TouristMarinaId { get; set; }
        public virtual TouristMarina? TouristMarina { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(Organization))]
        public required string OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
    }
}
