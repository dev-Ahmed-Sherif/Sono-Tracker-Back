using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Tracker.TripMarina;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        [ExcludeFromCodeCoverage]
        public void MapMarinaTrip()
        {
            CreateMap<TripMarina, TripMarinaDto>()
                .ForMember(des => des.TouristMarinaName, opt => opt.MapFrom(src => src.TouristMarina.NameAr))
                .ForMember(des => des.TouristMarinaCode, opt => opt.MapFrom(src => src.TouristMarina.Code))
                .ForMember(des => des.FloatingUnitNameAr, opt => opt.MapFrom(src => src.TripInformation.FloatingUnit.NameAr))
                .ForMember(des => des.FloatingUnitNameEn, opt => opt.MapFrom(src => src.TripInformation.FloatingUnit.NameEn))
                .ForMember(des => des.FloatingUnitCode, opt => opt.MapFrom(src => src.TripInformation.FloatingUnit.Code))
                .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                .ReverseMap();

            CreateMap<TripMarina, EditTripMarinaDto>()
                .ForMember(des => des.TouristMarinaName, opt => opt.MapFrom(src => src.TouristMarina.NameAr))
                .ForMember(des => des.TouristMarinaCode, opt => opt.MapFrom(src => src.TouristMarina.Code))
                .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                .ReverseMap();

            CreateMap<AddTripMarinaDto, TripMarina>()
                .ForMember(des => des.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
