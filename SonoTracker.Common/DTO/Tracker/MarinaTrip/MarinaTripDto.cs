using SonoTracker.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.MarinaTrip
{
    public class MarinaTripDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set ; }
        public Guid TouristMarinaId { get; set; }
        public string TouristMarinaName{ get; set; }

        public string TouristMarinaCode { get; set; }
        public Guid TripInformationId { get; set; }
        public string TripInformationCode { get; set; }
        public string FloatingUnitNameAr { get; set; }
        public string FloatingUnitNameEn { get; set; }
        public string FloatingUnitCode { get; set; }
    }
}
