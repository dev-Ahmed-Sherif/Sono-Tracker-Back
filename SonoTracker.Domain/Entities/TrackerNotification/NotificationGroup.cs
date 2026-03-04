using SonoTracker.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace SonoTracker.Domain.Entities.TrackerNotification
{
    public class NotificationGroup : Lookup<string>
    {
        public NotificationGroup()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        public virtual HashSet<Notification> Notifications { get; set; } = [];

    }
}
