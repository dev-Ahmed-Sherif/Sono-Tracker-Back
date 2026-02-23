using SonoTracker.Common.DTO.Tracker.OrganizationStaff;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapOrganizationStaff()
        {
            CreateMap<OrganizationStaff, OrganizationStaffDto>()
                 .ForMember(des => des.GenderName, opt => opt.MapFrom(src => src.Gender.GetName()))
                 .ForMember(des => des.IDTypeName, opt => opt.MapFrom(src => src.IDType.GetName()))
                 .ForMember(des => des.NationalityName, opt => opt.MapFrom(src => src.Nationality.NameAr))
                 .ForMember(des => des.OrganizationName, opt => opt.MapFrom(src => src.Organization.NameAr))
                 .ReverseMap();

            CreateMap<OrganizationStaff, EditOrganizationStaffDto>().ReverseMap();

            CreateMap<OrganizationStaff, AddOrganizationStaffDto>().ReverseMap();
        }
    }
}
