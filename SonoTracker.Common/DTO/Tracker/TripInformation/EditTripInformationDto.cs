using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SonoTracker.Common.Core;

namespace SonoTracker.Common.DTO.Tracker.TripInformation
{
    public class EditTripInformationDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }

        public DateTime SartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Code { get; set; }

        public Guid FloatingUnitId { get; set; }

        public int StaffNumber { get; set; }

        public int PassengerNumber { get; set; }

        public Guid RouteId { get; set; }
        public string PassengerAttachment { get; set; }

    }
}
