using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.InspectionClause;
using SonoTracker.Common.DTO.Tracker.InspectionClause.Parameters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.InspectionClause
{
    public interface IInspectionClauseService : IBaseService<Domain.Entities.Tracker.InspectionClause, AddInspectionClauseDto, EditInspectionClauseDto, InspectionClauseDto, string, string>
    {
        Task<PagingResult> GetAllPagedAsync(BaseParam<InspectionClauseFilter> filter, CancellationToken cancellationToken = default);

        Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}
