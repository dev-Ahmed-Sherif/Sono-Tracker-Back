using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.TestAttachment;

namespace SonoTracker.Common.DTO.Test
{
    [ExcludeFromCodeCoverage]
    public  class TestDto : IEntityDto<string>
    {
        public string? Id { get; set; }

        public string NameEn { get; set; }

        public string NameAr { get; set; }

        public List<TestAttachmentDto> TestAttachments { get; set; } = new();

        public DateTime CreatedDate { get; set; }
    }
}
