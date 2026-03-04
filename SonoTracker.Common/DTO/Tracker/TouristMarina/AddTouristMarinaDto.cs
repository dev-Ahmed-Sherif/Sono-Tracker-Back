using SonoTracker.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.TouristMarina
{
    public class AddTouristMarinaDto : IEntityDto<string>
    {
        public string? Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; }
        public string TownId { get; set; }
        public string Url { get; set; }

        public float Length { get; set; }

        public string NorthSide { get; set; }

        public string SouthSide { get; set; }

        public string GeoPointId { get; set; }
        public string Note { get; set; }

    }
}
