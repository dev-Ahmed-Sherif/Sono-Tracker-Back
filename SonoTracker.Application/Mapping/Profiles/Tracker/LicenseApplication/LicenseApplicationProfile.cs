using SonoTracker.Common.DTO.Tracker.LicenseApplication;
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
        public void MapLicenseApplication()
        {
            CreateMap<LicenseApplication, LicenseApplicationDto>().ReverseMap();
            CreateMap<LicenseApplication, AddLicenseApplicationDto>().ReverseMap();
            CreateMap<LicenseApplication, EditLicenseApplicationDto>().ReverseMap();
        }
    }
}
