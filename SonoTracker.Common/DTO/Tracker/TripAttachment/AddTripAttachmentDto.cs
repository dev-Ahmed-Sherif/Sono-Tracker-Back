using SonoTracker.Common.Core;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripAttachment
{
    [ExcludeFromCodeCoverage]
    public class AddTripAttachmentDto : IEntityDto<string>
    {
        public string? Id { get; set; }
        public string AttachmentId { get; set; }
        public string TripInformationId { get; set; }
    }
}
