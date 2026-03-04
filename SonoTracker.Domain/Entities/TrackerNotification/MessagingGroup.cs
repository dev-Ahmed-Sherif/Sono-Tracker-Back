using SonoTracker.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace SonoTracker.Domain.Entities.TrackerNotification
{
    public class MessagingGroup : Lookup<string>
    {
        public MessagingGroup()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        public virtual HashSet<Message> Messages { get; set; } = [];

    }
}
