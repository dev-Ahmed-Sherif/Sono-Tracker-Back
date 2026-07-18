using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripNationality.Parameters
{
    [ExcludeFromCodeCoverage]
    public class TripNationalityFilter
    {
        public string? NationalityId { get; set; }
        public string? TripInformationId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
