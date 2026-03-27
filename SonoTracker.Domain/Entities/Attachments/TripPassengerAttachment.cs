using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Domain.Entities.Attachments
{
    public class TripPassengerAttachment : BaseEntity<string>
    {
        public TripPassengerAttachment()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [MaxLength(50)]
        public required string FileId { get; set; }

        public required string FileName { get; set; }

        public required string Extension { get; set; }

        public required string Size { get; set; }

        public bool IsPublic { get; set; }

        public required string AttachmentDisplaySize { get; set; }

        public required string Url { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(TripInformation))]
        public required string TripInformationId { get; set; }
        public virtual TripInformation? TripInformation { get; set; }
    }
}
