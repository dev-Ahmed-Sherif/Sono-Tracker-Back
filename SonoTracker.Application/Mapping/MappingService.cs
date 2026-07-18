using AutoMapper;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService : Profile
    {
        public MappingService()
        {
            MapAccidentType();
            MapCity();
            MapTown();
            MapRoute();
            MapMaintenanceType();
            MapUnitType();
            MapOrganizationCategory();
            MapNationality();
            MapInspectionType();
            MapGeoPoint();
            MapGovernorate();
            MapOrganization();
            MapOrganizationStaff();
            MapFloatingUnit();
            MapFloatingUnitStaff();
            MapTripAttachment();
            MapTripInformation();
            MapFloatingUnitOrganization();
            MapInspection();
            MapTouristMarina();
            MapMarinaOrganization();
            MapLicenseApplication();
            MapTripGeo();
            MapMarinaTrip();
            MapTripNationality();
            MapTripStaff();
            MapTripPassenger();
            MapMaintenance();
            MapAccident();
            MapUser();
            MapInspectionClause();
            MapInspectionFloatingUnitClause();
        }
    }
}