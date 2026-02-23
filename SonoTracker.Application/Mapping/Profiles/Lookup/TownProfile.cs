using SonoTracker.Common.DTO.Lookup.Town;
using SonoTracker.Domain.Entities.Lookups;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapTown()
        {
            CreateMap<Town, TownDto>()
                .ForMember(dest => dest.CityName, cfg => cfg.MapFrom(src => src.City.NameAr))
                .ForMember(dest => dest.GovernorateName, cfg => cfg.MapFrom(src => src.Governorate.Name));

            CreateMap<Town, EditTownDto>().ReverseMap();
            CreateMap<Town, AddTownDto>().ReverseMap();
        }
    }
}
