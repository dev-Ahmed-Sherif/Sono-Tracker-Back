using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.TripInformation.Parameters
{
    public class TripInformationFilter
    {
        public DateTime? SartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Code { get; set; }

        public Guid? FloatingUnitId { get; set; }

        public Guid? RouteId { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
