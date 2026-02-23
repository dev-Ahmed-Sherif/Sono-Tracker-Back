using SonoTracker.Domain.Entities.Attachments;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class Organization : BaseEntity<Guid>
    {
        [MaxLength(100)]
        public string Code { get; set; }
        [MaxLength(100)]
        public string NameAr { get; set; }

        [MaxLength(100)]
        public string? NameEn { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Fax { get; set; }

        [MaxLength(100)]
        public string? Mobile { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }
        public Guid? NationalityId { get; set; }
        public virtual Nationality Nationality { get; set; }
        public OrganizationType OrganizationTypeId { get; set; }
        public AppliedOn? AppliedOn { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsReport { get; set; }

        [MaxLength(50)]
        public string? CommercialRegistrationNumber { get; set; }
        public string? WebSiteAddress { get; set; }

        [MaxLength(50)]
        public int? TouristMarinaNumber { get; set; }
        public Guid? InspectionTypeId { get; set; }
        public virtual Lookups.InspectionType? InspectionType { get; set; }
        public virtual ICollection<OrganizationStaff> OrganizationStaffs { get; set; } = new HashSet<OrganizationStaff>();
        public virtual ICollection<MarinaOrganization> MarinaOwners { get; set; } = new HashSet<MarinaOrganization>();
        public virtual ICollection<FloatingUnitOrganization> FloatingUnitOrganizations { get; set; } = new HashSet<FloatingUnitOrganization>();
        public virtual ICollection<Inspection> Inspections { get; set; } = new HashSet<Inspection>();
        public virtual ICollection<LicenseApplication> LicenseApplications { get; set; } = new HashSet<LicenseApplication>();
        public virtual ICollection<Accident> Accidents { get; set; } = new HashSet<Accident>();
    }
}
