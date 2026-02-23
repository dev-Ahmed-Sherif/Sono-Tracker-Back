using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.MarinaTrip.Parameters
{
    [ExcludeFromCodeCoverage]
    public  class MarinaTripFilter 
    {
        public Guid? TouristMarinaId { get; set; }
        public Guid? TripInformationId { get; set; }
        public Guid? FloatingUnitId { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}
