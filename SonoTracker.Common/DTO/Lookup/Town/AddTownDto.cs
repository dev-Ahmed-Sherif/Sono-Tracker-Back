using SonoTracker.Common.DTO.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace SonoTracker.Common.DTO.Lookup.Town
{
    public class AddTownDto : LookupDto<Guid?>
    {
        [Required]
        public required Guid CityId { get; set; }
        [Required]
        public required Guid GovernorateId { get; set; }
    }
}
