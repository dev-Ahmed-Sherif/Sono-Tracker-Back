using SonoTracker.Domain.Entities.Lookups;
using System.Collections.Generic;
using AccidentType = SonoTracker.Domain.Entities.Lookups.AccidentType;
using Nationality = SonoTracker.Domain.Entities.Lookups.Nationality;

namespace SonoTracker.Infrastructure.DataInitializer
{
    public interface IDataInitializer
    {
        IEnumerable<Nationality> SeedNationalitiesAsync();

        IEnumerable<AccidentType> SeedAccidentTypesAsync();

        //IEnumerable<Status> SeedStatusesAsync();
    }
}