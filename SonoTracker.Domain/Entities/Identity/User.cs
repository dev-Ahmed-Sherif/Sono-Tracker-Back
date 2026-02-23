
using Microsoft.AspNetCore.Identity;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; } = string.Empty;
        public bool IsLogedIn { get; set; }
        public Guid? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public Guid? FloatingUnitId { get; set; }
        public virtual FloatingUnit FloatingUnit { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
