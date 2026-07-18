using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Tracker.TripNationality;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        [ExcludeFromCodeCoverage]
        public void MapTripNationality()
        {
            CreateMap<TripNationality, TripNationalityDto>()
                .ForMember(des => des.NationalityNameAr, opt => opt.MapFrom(src => src.Nationality.NameAr))
                .ForMember(des => des.NationalityNameEn, opt => opt.MapFrom(src => src.Nationality.NameEn))
                .ForMember(des => des.NationalityCode, opt => opt.MapFrom(src => src.Nationality.Code))
                .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                .ReverseMap();

            CreateMap<TripNationality, EditTripNationalityDto>()
                .ForMember(des => des.NationalityNameAr, opt => opt.MapFrom(src => src.Nationality.NameAr))
                .ForMember(des => des.NationalityNameEn, opt => opt.MapFrom(src => src.Nationality.NameEn))
                .ForMember(des => des.NationalityCode, opt => opt.MapFrom(src => src.Nationality.Code))
                .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                .ReverseMap();

            CreateMap<AddTripNationalityDto, TripNationality>()
                .ForMember(des => des.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
