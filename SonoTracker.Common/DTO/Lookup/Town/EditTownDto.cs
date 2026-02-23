using SonoTracker.Common.DTO.Base;
using System;

namespace SonoTracker.Common.DTO.Lookup.Town
{
    public class EditTownDto : LookupDto<Guid?>
    {
        public Guid CityId { get; set; }

        public Guid GovernorateId { get; set; }

    }
}
