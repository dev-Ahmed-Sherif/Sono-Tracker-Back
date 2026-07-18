using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripInformation.Parameters
{
    [ExcludeFromCodeCoverage]
    public class TripInformationFilter
    {
        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public string Code { get; set; }

        public string? FloatingUnitId { get; set; }

        public string? RouteId { get; set; }

        public string? GovernorateId { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
