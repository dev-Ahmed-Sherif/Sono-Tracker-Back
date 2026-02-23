using SonoTracker.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class MarinaOrganization : BaseEntity<Guid>
    {
        [MaxLength(100)]
        public string LicenseNumber { get; set; }
        public Guid TouristMarinaId { get; set; }
        public virtual TouristMarina TouristMarina { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
    }
}
