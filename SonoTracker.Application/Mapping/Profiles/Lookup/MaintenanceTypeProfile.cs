using SonoTracker.Common.DTO.Lookup.MaintenanceType;
using SonoTracker.Domain.Entities.Lookups;


namespace SonoTracker.Application.Mapping
{
     public partial class MappingService
    {
       public void MapMaintenanceType()
        {
            CreateMap<MaintenanceType, MaintenanceTypeDto>().ReverseMap();
            
            CreateMap<MaintenanceType, EditMaintenanceTypeDto>().ReverseMap();
            
            CreateMap<MaintenanceType, AddMaintenanceTypeDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

        }
    }
}
