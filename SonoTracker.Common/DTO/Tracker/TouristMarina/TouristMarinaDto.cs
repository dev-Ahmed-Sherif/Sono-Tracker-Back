using SonoTracker.Common.Core;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.TouristMarina
{
    public class TouristMarinaDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set ; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; }
        public Guid TownId { get; set; }
        public string TownNameAr { get; set; }
        public string TownNameEn { get; set; }
        public string Url { get; set; }
        public float Length { get; set; }
        public string NorthSide { get; set; }
        public string SouthSide { get; set; }
        public Guid GeoPointId { get; set; }
        public string GeoPointNorth { get; set; }
        public string GeoPointEast { get; set; }
        public string Note { get; set; }
    }
}
