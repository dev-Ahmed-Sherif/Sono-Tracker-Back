using SonoTracker.Common.Core;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripGeo
{
    [ExcludeFromCodeCoverage]
    public class AddTripGeoDto : IEntityDto<string>
    {
        public string? Id { get; set; }
        public string GeoPointId { get; set; }
        public string TripInformationId { get; set; }
    }
}
