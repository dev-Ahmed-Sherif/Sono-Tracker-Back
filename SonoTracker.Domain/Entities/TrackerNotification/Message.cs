using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Identity;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.TrackerNotification
{
    [ExcludeFromCodeCoverage]

    public class Message : BaseEntity<Guid>
    {
        public string Content { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public virtual User? Sender { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public virtual User? Receiver { get; set; }

        public Guid? MessagingGroupId { get; set; }
        public virtual MessagingGroup NotificationGroup { get; set; }
        public bool IsRead { get; set; }
    }
}
