using SonoTracker.Common.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripMarina
{
    [ExcludeFromCodeCoverage]
    public class EditTripMarinaDto : IEntityDto<string>
    {
        public string Id { get; set; }
        public string TouristMarinaId { get; set; }
        public string TouristMarinaName { get; set; }
        public string TouristMarinaCode { get; set; }
        public string TripInformationId { get; set; }
        public string TripInformationCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedById { get; set; }
    }
}
