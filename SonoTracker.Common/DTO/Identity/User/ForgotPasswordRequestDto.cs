using System.ComponentModel.DataAnnotations;

namespace SonoTracker.Common.DTO.Identity.User
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        public required string Identifier { get; set; }
    }
}
