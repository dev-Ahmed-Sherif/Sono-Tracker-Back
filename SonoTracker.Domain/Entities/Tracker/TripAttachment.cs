
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class TripAttachment : BaseEntity<string>
    {
        public TripAttachment()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [Required, MaxLength(50)]
        [ForeignKey(nameof(Attachment))]
        public required string AttachmentId { get; set; }
        public virtual Attachment? Attachment { get; set; }

        [Required, MaxLength(50)]
        [ForeignKey(nameof(TripInformation))]
        public required string TripInformationId { get; set; }
        public virtual TripInformation? TripInformation { get; set; }
    }
}
