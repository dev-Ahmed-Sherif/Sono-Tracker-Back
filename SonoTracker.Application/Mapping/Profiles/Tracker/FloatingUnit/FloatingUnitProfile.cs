using SonoTracker.Common.DTO.Tracker.FloatingUnit;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapFloatingUnit()
        {
            CreateMap<FloatingUnit, FloatingUnitDto>()
                 .ForMember(des => des.UnitCategory, opt => opt.MapFrom(src => src.UnitType.UnitCategory.GetName()))
                 .ForMember(des => des.UnitType, opt => opt.MapFrom(src => src.UnitType.NameAr))
                 .ReverseMap();
            CreateMap<FloatingUnit, EditFloatingUnitDto>().ReverseMap();
            CreateMap<FloatingUnit, AddFloatingUnitDto>().ReverseMap();
        }
    }
}
