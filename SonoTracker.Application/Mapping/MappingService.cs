using AutoMapper;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService : Profile
    {
        public MappingService()
        {
            MapCompany();
            MapAccidentType();
            MapCity();
            MapTown();
            MapRoute();
            MapMaintenanceType();
            MapUnitType();
            MapNationality();
            MapInspectionType();
            MapGeoPoint();
            MapGovernorate();
            MapGovernorateGeoPoint();
            MapOrganization();
            MapOrganizationStaff();
            MapFloatingUnit();
            MapFloatingUnitStaff();
            MapTripInformation();
            MapFloatingUnitOrganization();
            MapInspection();
            MapTouristMarina();
            MapMarinaOrganization();
            MapLicenseApplication();
            MapTripGeo();
            MapMarinaTrip();
            MapMaintenance();
            MapAccident();
            MapUser();

        }
    }
}