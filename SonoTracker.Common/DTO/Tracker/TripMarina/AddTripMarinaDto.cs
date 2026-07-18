using SonoTracker.Common.Core;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripMarina
{
    [ExcludeFromCodeCoverage]
    public class AddTripMarinaDto : IEntityDto<string>
    {
        public string? Id { get; set; }
        public string TouristMarinaId { get; set; }
        public string TripInformationId { get; set; }
    }
}
