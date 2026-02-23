using System.ComponentModel.DataAnnotations;

namespace SonoTracker.Common.DTO.Identity.User
{
    public class ChangeUserPersonalDataDto
    {
        [Required]
        public required string UserName { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string OldPassword { get; set; }
        [Required]
        public required string NewPassword { get; set; }
        [Required, Compare("NewPassword")]
        public required string ConfirmPassword { get; set; }
    }
}
