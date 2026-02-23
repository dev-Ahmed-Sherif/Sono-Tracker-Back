using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Domain.Entities.Attachments
{
    public class PassengerTripAttachment : BaseEntity<Guid>
    {
        public Guid FileId { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public string Size { get; set; }

        public bool IsPublic { get; set; }

        public string AttachmentDisplaySize { get; set; }

        public string Url { get; set; }

        public Guid TripInformationId { get; set; }

        public TripInformation TripInformation { get; set; }
    }
}
