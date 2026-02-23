using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class OrganizationStaff : BaseEntity<Guid>
    {
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
        public Guid? NationalityId { get; set; }
        public virtual Nationality Nationality { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public bool IsDelegate { get; set; }
        public string? DelegateAttachment { get; set; }
    }
}