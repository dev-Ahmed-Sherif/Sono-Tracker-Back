using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.TestAttachment
{
    [ExcludeFromCodeCoverage]
    public class EditTestAttachmentDto
    {
        public string? Id { get; set; }

        public Guid FileId { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public string Size { get; set; }

        public bool IsPublic { get; set; }

        public string Url { get; set; }

        public string AttachmentDisplaySize { get; set; }

        public Guid TestId { get; set; }
    }
}
