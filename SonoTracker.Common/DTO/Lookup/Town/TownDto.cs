using SonoTracker.Common.DTO.Base;
using System;

namespace SonoTracker.Common.DTO.Lookup.Town
{
    public class TownDto : LookupDto<string>
    {
        public string CityId { get; set; }

        public string GovernorateId { get; set; }
      
        public string City { get; set; }
      
        public string Governorate { get; set; }
       
        public DateTime CreatedDate { get; set; }
    }
}
