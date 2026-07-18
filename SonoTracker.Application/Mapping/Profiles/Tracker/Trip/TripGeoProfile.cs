using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Tracker.TripGeo;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        [ExcludeFromCodeCoverage]
        public void MapTripGeo()
        {
            CreateMap<TripGeo, TripGeoDto>()
                .ForMember(des => des.GeoPointNorth, opt => opt.MapFrom(src => src.GeoPoint.North))
                .ForMember(des => des.GeoPointEast, opt => opt.MapFrom(src => src.GeoPoint.East))
                .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                .ForMember(des => des.FloatingUnitNameAr, opt => opt.MapFrom(src => src.TripInformation.FloatingUnit.NameAr))
                .ReverseMap();

            CreateMap<TripGeo, EditTripGeoDto>()
                .ForMember(des => des.GeoPointNorth, opt => opt.MapFrom(src => src.GeoPoint.North))
                .ForMember(des => des.GeoPointEast, opt => opt.MapFrom(src => src.GeoPoint.East))
                .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                .ForMember(des => des.FloatingUnitNameAr, opt => opt.MapFrom(src => src.TripInformation.FloatingUnit.NameAr))
                .ReverseMap();

            CreateMap<AddTripGeoDto, TripGeo>()
                .ForMember(des => des.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
