using System;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Domain.Entities.Attachments
{
    public class OrganizationAttachment : BaseEntity<Guid>
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Size { get; set; }
        public bool IsPublic { get; set; }
        public string AttachmentDisplaySize { get; set; }
        public string Url { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
