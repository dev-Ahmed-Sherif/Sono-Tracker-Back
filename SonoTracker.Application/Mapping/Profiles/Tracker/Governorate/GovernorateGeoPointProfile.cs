using SonoTracker.Common.DTO.Tracker.Governorate;
using SonoTracker.Common.DTO.Tracker.GovernorateGeoPoint;
using SonoTracker.Common.DTO.Tracker.Organization;
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
        public void MapGovernorateGeoPoint()
        {
            CreateMap<GovernorateGeoPoint, GovernorateGeoPointDto>()
                 .ForMember(des => des.GeoPointEast, opt => opt.MapFrom(src => src.GeoPoint.East))
                 .ForMember(des => des.GeoPointNorth, opt => opt.MapFrom(src => src.GeoPoint.North))
                 .ForMember(des => des.GovernorateName, opt => opt.MapFrom(src => src.Governorate.Name))
                 .ReverseMap();

            CreateMap<GovernorateGeoPoint, EditGovernorateGeoPointDto>().ReverseMap();
            CreateMap<GovernorateGeoPoint, AddGovernorateGeoPointDto>().ReverseMap();
        }
    }
}
