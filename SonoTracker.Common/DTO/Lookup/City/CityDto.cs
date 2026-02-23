using SonoTracker.Common.DTO.Base;
using System;

namespace SonoTracker.Common.DTO.Lookup.City
{
    public class CityDto : LookupDto<Guid?>
    {
        public DateTime CreatedDate { get; set; }
    }
}
