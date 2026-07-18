using SonoTracker.Common.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripAttachment
{
    [ExcludeFromCodeCoverage]
    public class TripAttachmentDto : IEntityDto<string>
    {
        public string Id { get; set; }
        public string AttachmentId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Size { get; set; }
        public string Url { get; set; }
        public bool IsPublic { get; set; }
        public string TripInformationId { get; set; }
        public string TripInformationCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedById { get; set; }
    }
}
