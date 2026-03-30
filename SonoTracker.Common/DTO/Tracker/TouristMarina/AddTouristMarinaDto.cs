using SonoTracker.Common.Core;

namespace SonoTracker.Common.DTO.Tracker.TouristMarina
{
    public class AddTouristMarinaDto : IEntityDto<string>
    {
        public string Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; }
        public string CityId { get; set; }
        public string Url { get; set; }
        public float Length { get; set; }
        public string NorthSide { get; set; }
        public string SouthSide { get; set; }
        public string GeoPointId { get; set; }
        public string Note { get; set; }
    }
}
