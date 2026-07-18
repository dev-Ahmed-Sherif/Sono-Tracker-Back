using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripAttachment;
using SonoTracker.Common.DTO.Tracker.TripAttachment.Parameters;

namespace SonoTracker.Application.Services.Tracker.TripAttachment
{
    public interface ITripAttachmentService : IBaseService<Entities.Tracker.TripAttachment, AddTripAttachmentDto, EditTripAttachmentDto, TripAttachmentDto, string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<TripAttachmentFilter> filter, CancellationToken cancellationToken = default);
        Task<IFinalResult> GetAllFilterAsync(TripAttachmentFilter filter, CancellationToken cancellationToken = default);
        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        Task<IFinalResult> DeleteRangeWithAttachIdRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default);
    }
}
