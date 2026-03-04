using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class OrganizationStaff : BaseEntity<string>
    {
        public OrganizationStaff()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Job { get; set; }
        [MaxLength(100)]
        public string Phone { get; set; }
        [MinLength(100)]
        public string Mobile { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string? PhoneExtension { get; set; }
        public Gender Gender { get; set; }
        public IDType IDType { get; set; }
        [MaxLength(20)]
        public string? Identity { get; set; }

        [MaxLength(50), ForeignKey(nameof(Nationality))]
        public string? NationalityId { get; set; }
        public virtual Nationality? Nationality { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(Organization))]
        public required string OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }
        public bool IsDelegate { get; set; }
        public string? DelegateAttachment { get; set; }
    }
}