using SonoTracker.Domain.Entities.Lookups;
using City = SonoTracker.Domain.Entities.Lookups.City;
using System.Collections.Generic;
using AccidentType = SonoTracker.Domain.Entities.Lookups.AccidentType;
using Governorate = SonoTracker.Domain.Entities.Lookups.Governorate;
using Nationality = SonoTracker.Domain.Entities.Lookups.Nationality;
using NotificationGroup = SonoTracker.Domain.Entities.TrackerNotification.NotificationGroup;

namespace SonoTracker.Infrastructure.DataInitializer
{
    public interface IDataInitializer
    {
        IEnumerable<Nationality> SeedNationalitiesAsync();

        IEnumerable<AccidentType> SeedAccidentTypesAsync();

        IEnumerable<Governorate> SeedGovernoratesAsync();

        IEnumerable<City> SeedCitiesAsync();

        IEnumerable<NotificationGroup> SeedNotificationGroupsAsync();

        //IEnumerable<Status> SeedStatusesAsync();
    }
}