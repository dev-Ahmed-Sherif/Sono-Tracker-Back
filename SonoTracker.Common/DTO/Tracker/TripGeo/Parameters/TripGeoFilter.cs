using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripGeo.Parameters
{
    [ExcludeFromCodeCoverage]
    public  class TripGeoFilter 
    {
        public Guid? TripInformationId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
