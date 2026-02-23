using SonoTracker.Domain.Entities.Base;
using System;

namespace SonoTracker.Domain.Entities.Identity
{
    public class RefreshToken:BaseEntity<Guid>
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
        public virtual User User { get; set; }
    }
}
