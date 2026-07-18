using SonoTracker.Common.Core;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripNationality
{
    [ExcludeFromCodeCoverage]
    public class AddTripNationalityDto : IEntityDto<string>
    {
        public string? Id { get; set; }
        public string NationalityId { get; set; }
        public string TripInformationId { get; set; }
    }
}
