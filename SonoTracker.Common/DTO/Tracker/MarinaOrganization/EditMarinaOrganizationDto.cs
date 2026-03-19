using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.MarinaOrganization
{

    [ExcludeFromCodeCoverage]
    public class EditMarinaOrganizationDto : IEntityDto<string>
    {
        public string Id { get; set; }
        public string TouristMarinaId { get; set; }
        public string TouristMarinaName { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public bool IsActive { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedById { get; set; }
    }
}
