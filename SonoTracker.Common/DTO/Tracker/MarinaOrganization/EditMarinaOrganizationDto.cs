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
    public class EditMarinaOrganizationDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public Guid TouristMarinaId { get; set; }
        public string TouristMarinaName { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public bool IsActive { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
}
