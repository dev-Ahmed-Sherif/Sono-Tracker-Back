using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.TrackerNotification;
using System;
using System.Collections.Generic;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class NotificationGroup : Lookup<Guid>
    {
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();

    }
}
