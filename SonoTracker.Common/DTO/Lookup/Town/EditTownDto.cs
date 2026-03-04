using SonoTracker.Common.DTO.Base;
using System;

namespace SonoTracker.Common.DTO.Lookup.Town
{
    public class EditTownDto : LookupDto<string>
    {
        public string CityId { get; set; }

        public string GovernorateId { get; set; }

    }
}
