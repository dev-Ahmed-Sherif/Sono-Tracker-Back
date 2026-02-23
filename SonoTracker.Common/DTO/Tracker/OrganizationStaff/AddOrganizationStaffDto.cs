using Microsoft.AspNetCore.Http;
using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.OrganizationStaff
{
    [ExcludeFromCodeCoverage]
    public class AddOrganizationStaffDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }

        [Required, MaxLength(100), 
         RegularExpression("^[A-Za-z\u0600-\u06FF\u0660-\u0669\\s]{1,50}$",
         ErrorMessage = "Number Or Special Chars Not Allowed and length less than 50")]
        public string Name { get; set; }

        [Required, MaxLength(100), 
         RegularExpression("^[A-Za-z\u0600-\u06FF\u0660-\u0669\\s]{1,30}$", 
         ErrorMessage = "Number Or Special Chars Not Allowed and length less than 30")]
        public string Job { get; set; }

        [Phone]
        public string Phone { get; set; }
        
        [Required, Phone]
        public required string Mobile { get; set; }
        
        [RegularExpression("^[0-9]{0,5}$", ErrorMessage = "Number Only and length less than 5")]
        public string PhoneExtension { get; set; }
        
        [Required, EmailAddress]
        public required string Email { get; set; }

        public Gender Gender { get; set; }
        
        public IDType IDType { get; set; }

        [RegularExpression("^[A-Za-z0-9]{0,45}$", 
         ErrorMessage = "Identity Number Only Contain Numbers and Letters " +
         "No Special Chars Allowed and length less than 40 Letter or Number")]
        public string Identity { get; set; }
        
        public Guid? NationalityId { get; set; }
        
        [Required]
        public required Guid OrganizationId { get; set; }
        
        public bool IsDelegate { get; set; }
        
        public IFormFile DelegateAttachment { get; set; }
    }
}
