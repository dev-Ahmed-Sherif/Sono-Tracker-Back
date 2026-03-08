using System.Collections.Generic;
using System.IO;
using SonoTracker.Common.Extensions;
using AccidentType = SonoTracker.Domain.Entities.Lookups.AccidentType;
using Nationality = SonoTracker.Domain.Entities.Lookups.Nationality;

namespace SonoTracker.Infrastructure.DataInitializer
{
    public class DataInitializer(string contentRootPath) : IDataInitializer
    {
        public IEnumerable<Nationality> SeedNationalitiesAsync()
        {
            var path = Path.Combine(contentRootPath, "Seed", "Nationalities.json");
            var dataText = File.ReadAllText(path);
            var nationalities = Seeder<List<Nationality>>.SeedIt(dataText);
            return nationalities ?? [];
        }

        public IEnumerable<AccidentType> SeedAccidentTypesAsync()
        {
            var path = Path.Combine(contentRootPath, "Seed", "AccidentTypes.json");
            var dataText = File.ReadAllText(path);
            var accidentTypes = Seeder<List<AccidentType>>.SeedIt(dataText);
            return accidentTypes ?? [];
        }

        //public IEnumerable<Status> SeedStatusesAsync()
        //{
        //    var dataText = System.IO.File.ReadAllText(@"Seed/Statuses.json");
        //    var statuses = Seeder<List<Status>>.SeedIt(dataText);
        //    return statuses;
        //}
    }
}