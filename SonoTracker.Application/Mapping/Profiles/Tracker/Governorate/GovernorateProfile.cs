using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Common.DTO.Tracker.Governorate;
using SonoTracker.Common.DTO.Tracker.Organization;
using SonoTracker.Domain.Entities.Lookups;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapGovernorate()
        {
            CreateMap<Governorate, GovernorateDto>().ReverseMap();
            CreateMap<Governorate, EditGovernorateDto>().ReverseMap();
            CreateMap<Governorate, AddGovernorateDto>().ReverseMap();
        }
    }
}
