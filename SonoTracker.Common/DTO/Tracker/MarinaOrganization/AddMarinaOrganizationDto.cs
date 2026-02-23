using SonoTracker.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.MarinaOrganization
{
    public class AddMarinaOrganizationDto : IEntityDto<Guid?>
    {
        public Guid? Id { get ; set; }
       
        public Guid TouristMarinaId { get; set; }
        public Guid OrganizationId { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(100)]
        public string LicenseNumber { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

    }
}
