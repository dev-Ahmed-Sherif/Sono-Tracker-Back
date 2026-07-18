using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Tracker.TripInformation;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        [ExcludeFromCodeCoverage]
        public void MapTripInformation()
        {
            CreateMap<TripInformation, TripInformationDto>()
                 .ForMember(des => des.RouteName, opt => opt.MapFrom(src => src.Route.NameAr))
                 .ForMember(des => des.FloatingUnitNameAr, opt => opt.MapFrom(src => src.FloatingUnit.NameAr))
                 .ForMember(des => des.FloatingUnitNameEn, opt => opt.MapFrom(src => src.FloatingUnit.NameEn))
                 .ForMember(des => des.FloatingUnitCode, opt => opt.MapFrom(src => src.FloatingUnit.Code))
                 .ForMember(des => des.GovernorateNameAr, opt => opt.MapFrom(src => src.Governorate.NameAr))
                 .ReverseMap()
                 .ForMember(des => des.TripAttachments, opt => opt.Ignore())
                 .ForMember(des => des.NationalityTrips, opt => opt.Ignore())
                 .ForMember(des => des.TripMarinas, opt => opt.Ignore());

            CreateMap<TripInformation, EditTripInformationDto>()
                 .ForMember(des => des.RouteName, opt => opt.MapFrom(src => src.Route.NameAr))
                 .ForMember(des => des.FloatingUnitNameAr, opt => opt.MapFrom(src => src.FloatingUnit.NameAr))
                 .ForMember(des => des.FloatingUnitNameEn, opt => opt.MapFrom(src => src.FloatingUnit.NameEn))
                 .ForMember(des => des.FloatingUnitCode, opt => opt.MapFrom(src => src.FloatingUnit.Code))
                 .ForMember(des => des.GovernorateNameAr, opt => opt.MapFrom(src => src.Governorate.NameAr))
                 .ReverseMap()
                 .ForMember(des => des.TripAttachments, opt => opt.Ignore())
                 .ForMember(des => des.NationalityTrips, opt => opt.Ignore())
                 .ForMember(des => des.TripMarinas, opt => opt.Ignore());

            CreateMap<AddTripInformationDto, TripInformation>()
                .ForMember(des => des.Id, opt => opt.Ignore())
                .ForMember(des => des.PassengerAttachment, opt => opt.Ignore())
                .ForMember(des => des.StaffAttachment, opt => opt.Ignore())
                .ForMember(des => des.TripAttachments, opt => opt.Ignore())
                .ForMember(des => des.TripPassengers, opt => opt.Ignore())
                .ForMember(des => des.TripStaffs, opt => opt.Ignore())
                .ForMember(des => des.NationalityTrips, opt => opt.Ignore())
                .ForMember(des => des.TripMarinas, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(des => des.PassengerAttachment, opt => opt.Ignore())
                .ForMember(des => des.StaffAttachment, opt => opt.Ignore())
                .ForMember(des => des.OtherAttachment, opt => opt.Ignore());
        }
    }
}
