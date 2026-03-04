using SonoTracker.Domain.Entities.Attachments;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class Organization : BaseEntity<string>
    {
        public Organization()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

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

        [MaxLength(50), ForeignKey(nameof(Nationality))]
        public string? NationalityId { get; set; }
        public virtual Nationality? Nationality { get; set; }
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

        [MaxLength(50), ForeignKey(nameof(InspectionType))]
        public string? InspectionTypeId { get; set; }
        public virtual Lookups.InspectionType? InspectionType { get; set; }
        public virtual HashSet<OrganizationStaff> OrganizationStaffs { get; set; } = [];
        public virtual HashSet<MarinaOrganization> MarinaOwners { get; set; } = [];
        public virtual HashSet<FloatingUnitOrganization> FloatingUnitOrganizations { get; set; } = [];
        public virtual HashSet<Inspection> Inspections { get; set; } = [];
        public virtual HashSet<LicenseApplication> LicenseApplications { get; set; } = [];
        public virtual HashSet<Accident> Accidents { get; set; } = [];
    }
}
