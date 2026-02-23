using System;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Domain.Entities.Base;

namespace SonoTracker.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class TestAttachment : BaseEntity<Guid>
    {
        public Guid FileId { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public string Size { get; set; }

        public bool IsPublic { get; set; }

        public string AttachmentDisplaySize { get; set; }

        public string Url { get; set; }

        public Guid TestId { get; set; }

        public virtual Test Test { get; set; }

    }
}
