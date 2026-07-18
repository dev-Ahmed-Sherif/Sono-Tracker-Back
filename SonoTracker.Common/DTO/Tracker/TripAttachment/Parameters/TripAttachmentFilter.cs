using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripAttachment.Parameters
{
    [ExcludeFromCodeCoverage]
    public class TripAttachmentFilter
    {
        public string? TripInformationId { get; set; }
        public string? AttachmentId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
