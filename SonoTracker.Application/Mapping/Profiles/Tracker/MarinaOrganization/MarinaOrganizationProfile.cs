using SonoTracker.Common.DTO.Reports.TouristMarina;
using SonoTracker.Common.DTO.Tracker.MarinaOrganization;
using SonoTracker.Common.DTO.Tracker.TouristMarina;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapMarinaOrganization()
        {
            CreateMap<MarinaOrganization, MarinaOrganizationDto>().ReverseMap();
            CreateMap<MarinaOrganization, AddMarinaOrganizationDto>().ReverseMap();
            CreateMap<MarinaOrganization, EditMarinaOrganizationDto>().ReverseMap();
            CreateMap<MarinaOrganization, TouristMarinaReportDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TouristMarina.Name))
                .ForMember(dest => dest.NameOwner, opt => opt.MapFrom(src => src.Organization.NameAr))
                .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.TouristMarina.Length))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.TouristMarina.Url))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(src => src.TouristMarina.Town.NameAr))
                .ForMember(dest => dest.North, opt => opt.MapFrom(src => src.TouristMarina.GeoPoint.North))
                .ForMember(dest => dest.East, opt => opt.MapFrom(src => src.TouristMarina.GeoPoint.East))
                .ReverseMap();
        }
    }
}
