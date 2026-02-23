using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.TrackerNotification;
using System;
using System.Collections.Generic;

namespace SonoTracker.Domain.Entities.Lookups
{
    public class MessagingGroup : Lookup<Guid>
    {
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();

    }
}
