using SonoTracker.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripStaff.Parameters
{
    [ExcludeFromCodeCoverage]
    public class TripStaffFilter
    {
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public Gender? Gender { get; set; }
        public IDType? IDType { get; set; }
        public string? Identity { get; set; }
        public string? TripInformationId { get; set; }
        public string? NationalityId { get; set; }
        public string? GovernorateId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
