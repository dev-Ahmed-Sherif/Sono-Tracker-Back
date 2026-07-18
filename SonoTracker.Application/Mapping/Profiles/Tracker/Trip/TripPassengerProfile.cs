using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Tracker.TripPassenger;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        [ExcludeFromCodeCoverage]
        public void MapTripPassenger()
        {
            CreateMap<TripPassenger, TripPassengerDto>()
                .ForMember(des => des.NationalityNameAr, opt => opt.MapFrom(src => src.Nationality.NameAr))
                .ForMember(des => des.NationalityNameEn, opt => opt.MapFrom(src => src.Nationality.NameEn))
                .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                .ForMember(des => des.GovernorateNameAr, opt => opt.MapFrom(src => src.Governorate.NameAr))
                .ForMember(des => des.GovernorateNameEn, opt => opt.MapFrom(src => src.Governorate.NameEn))
                .ReverseMap();

            CreateMap<TripPassenger, EditTripPassengerDto>()
                .ForMember(des => des.NationalityNameAr, opt => opt.MapFrom(src => src.Nationality.NameAr))
                .ForMember(des => des.NationalityNameEn, opt => opt.MapFrom(src => src.Nationality.NameEn))
                .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                .ForMember(des => des.GovernorateNameAr, opt => opt.MapFrom(src => src.Governorate.NameAr))
                .ForMember(des => des.GovernorateNameEn, opt => opt.MapFrom(src => src.Governorate.NameEn))
                .ReverseMap();

            CreateMap<AddTripPassengerDto, TripPassenger>()
                .ForMember(des => des.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
