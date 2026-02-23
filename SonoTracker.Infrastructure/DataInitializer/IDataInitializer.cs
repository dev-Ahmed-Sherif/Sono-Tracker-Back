using System.Collections.Generic;
using Action = SonoTracker.Domain.Entities.Lookups.Action;
using Status = SonoTracker.Domain.Entities.Lookups.Status;

namespace SonoTracker.Infrastructure.DataInitializer
{
    public interface IDataInitializer
    {
        IEnumerable<Action> SeedActionsAsync();

        IEnumerable<Status> SeedStatusesAsync();
    }
}