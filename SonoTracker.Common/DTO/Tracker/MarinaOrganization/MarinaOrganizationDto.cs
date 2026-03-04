using SonoTracker.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.MarinaOrganization
{
    public class MarinaOrganizationDto : IEntityDto<string>
    {
        public string? Id { get; set; }
        public string TouristMarinaId { get; set; }
        public string TouristMarinaName { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public bool IsActive { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
