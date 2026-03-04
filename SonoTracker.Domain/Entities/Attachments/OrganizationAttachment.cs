using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Domain.Entities.Attachments
{
    public class OrganizationAttachment : BaseEntity<string>
    {
        public OrganizationAttachment()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [MaxLength(50)]
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Size { get; set; }
        public bool IsPublic { get; set; }
        public string AttachmentDisplaySize { get; set; }
        public string Url { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(Organization))]
        public required string OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }
    }
}
