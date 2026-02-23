using SonoTracker.Common.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripGeo
{
    [ExcludeFromCodeCoverage]
    public class EditTripGeoDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public Guid GeoPointId { get; set; }
        public string GeoPointNorth { get; set; }
        public string GeoPointEast { get; set; }
        public Guid TripInformationId { get; set; }
        public string TripInformationCode { get; set; }
        public string FloatingUnitNameAr { get; set; }
    }
}
