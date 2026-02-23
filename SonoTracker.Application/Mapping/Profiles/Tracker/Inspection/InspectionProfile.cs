using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Common.DTO.Tracker.GeneralInspection;
using SonoTracker.Common.DTO.Tracker.Inspection;
using SonoTracker.Common.DTO.Tracker.TripInformation;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapInspection()
        {
            CreateMap<Inspection, GeneralInspectionDto>()
                 .ForMember(des => des.InspectionTypeName, opt => opt.MapFrom(src => src.InspectionTypeId.GetName()))
                 .ForMember(des => des.OrganizationNameAr, opt => opt.MapFrom(src => src.Organization.NameAr))
                 .ForMember(des => des.OrganizationNameEn, opt => opt.MapFrom(src => src.Organization.NameEn))
                 .ForMember(des => des.FloatingUnitNameAr, opt => opt.MapFrom(src => src.TripInformation.FloatingUnit.NameAr))
                 .ForMember(des => des.FloatingUnitNameEn, opt => opt.MapFrom(src => src.TripInformation.FloatingUnit.NameEn))
                 .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                 .ReverseMap();
            CreateMap<Inspection, EditGeneralInspectionDto>().ReverseMap();
            CreateMap<Inspection, AddGeneralInspectionDto>().ReverseMap();
        }
    }
}
