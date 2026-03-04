using SonoTracker.Domain.Entities.Lookups;
using System.Collections.Generic;
using Nationality = SonoTracker.Domain.Entities.Lookups.Nationality;

namespace SonoTracker.Infrastructure.DataInitializer
{
    public interface IDataInitializer
    {
        IEnumerable<Nationality> SeedNationalitiesAsync();

        //IEnumerable<Status> SeedStatusesAsync();
    }
}