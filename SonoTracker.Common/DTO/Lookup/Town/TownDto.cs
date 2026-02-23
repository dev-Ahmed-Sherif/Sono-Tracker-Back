using SonoTracker.Common.DTO.Base;
using System;

namespace SonoTracker.Common.DTO.Lookup.Town
{
    public class TownDto : LookupDto<Guid?>
    {
        public Guid CityId { get; set; }

        public Guid GovernorateId { get; set; }
      
        public string CityName { get; set; } = string.Empty;
      
        public string GovernorateName { get; set; } = string.Empty;
       
        public DateTime CreatedDate { get; set; }
    }
}
