using SonoTracker.Common.DTO.Lookup.Town;
using SonoTracker.Domain.Entities.Lookups;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapTown()
        {
            CreateMap<Town, TownDto>()
                .ForMember(dest => dest.City, cfg => cfg.MapFrom(src => src.City.NameAr))
                .ForMember(dest => dest.Governorate, cfg => cfg.MapFrom(src => src.City.Governorate.Name));

            CreateMap<Town, EditTownDto>().ReverseMap();
            CreateMap<Town, AddTownDto>().ReverseMap();
        }
    }
}
