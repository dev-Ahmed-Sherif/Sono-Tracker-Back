using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Lookup.Attach;
using SonoTracker.Domain.Entities.Lookups;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.LookUp.Attachments
{
    public interface IAttachmentService : IBaseService<Attachment, AddAttachmentDto, EditAttachmentDto, AttachmentDto, string, string>
    {
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}
