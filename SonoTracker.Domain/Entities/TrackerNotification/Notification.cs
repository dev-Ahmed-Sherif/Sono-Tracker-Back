using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Entities.Base;
using System;
using SonoTracker.Domain.Entities.Identity;

namespace SonoTracker.Domain.Entities.TrackerNotification
{
    [ExcludeFromCodeCoverage]
    public class Notification : BaseEntity<Guid>
    {
        public string Content { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public virtual User? Sender { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public virtual User? Receiver { get; set; }

        public Guid? NotificationGroupId { get; set; }
        public virtual NotificationGroup NotificationGroup { get; set; }
        public bool IsRead { get; set; }
    }
}
