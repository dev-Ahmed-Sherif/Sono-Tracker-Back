using SonoTracker.Common.DTO.Tracker.Accident;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapAccident()
        {
            CreateMap<Accident, AccidentDto>()
                .ForMember(des => des.AccidentType, opt => opt.MapFrom(src => src.AccidentType.NameAr))
                .ForMember(des => des.FloatingUnit, opt => opt.MapFrom(src => src.FloatingUnit.NameAr))
                .ForMember(des => des.Organization, opt => opt.MapFrom(src => src.Organization.NameAr))
                .ForMember(des => des.Town, opt => opt.MapFrom(src => src.Town.NameAr))
                .ForMember(des => des.Case, opt => opt.MapFrom(src => src.CaseId.GetName()))
                .ReverseMap();

            CreateMap<Accident, EditAccidentDto>()
                .ForMember(des => des.AccidentType, opt => opt.MapFrom(src => src.AccidentType.NameAr))
                .ForMember(des => des.FloatingUnit, opt => opt.MapFrom(src => src.FloatingUnit.NameAr))
                .ForMember(des => des.Organization, opt => opt.MapFrom(src => src.Organization.NameAr))
                .ForMember(des => des.Town, opt => opt.MapFrom(src => src.Town.NameAr))
                .ForMember(des => des.Case, opt => opt.MapFrom(src => src.CaseId.GetName()))
                .ReverseMap();

            CreateMap<Accident, AddAccidentDto>().ReverseMap();
        }
    }
}
