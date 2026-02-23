using System.Collections.Generic;
using SonoTracker.Common.Extensions;
using Action = SonoTracker.Domain.Entities.Lookups.Action;
using Status = SonoTracker.Domain.Entities.Lookups.Status;

namespace SonoTracker.Infrastructure.DataInitializer
{
    public class DataInitializer : IDataInitializer
    {
        public IEnumerable<Action> SeedActionsAsync()
        {
            var dataText = System.IO.File.ReadAllText(@"Seed/Actions.json");
            var actions = Seeder<List<Action>>.SeedIt(dataText);
            return actions;
        }
        public IEnumerable<Status> SeedStatusesAsync()
        {
            var dataText = System.IO.File.ReadAllText(@"Seed/Statuses.json");
            var statuses = Seeder<List<Status>>.SeedIt(dataText);
            return statuses;
        }
    }
}