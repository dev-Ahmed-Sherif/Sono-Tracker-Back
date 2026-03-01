using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SonoTracker.Domain.Entities.Identity
{
    public class RefreshToken : BaseAudit<string>
    {
        public RefreshToken()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }
        public string? Token { get; set; }
        public DateTime ExpiryTime { get; set; }

        [Required, MaxLength(50)]
        [ForeignKey(nameof(Organization))]
        public required string UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
