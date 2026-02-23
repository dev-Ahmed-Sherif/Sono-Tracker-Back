using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Common.DTO.Base;

namespace SonoTracker.Common.DTO.Lookup.GeoPoint
{
    [ExcludeFromCodeCoverage]
    public class AddGeoPointDto : LookupDto<Guid?>
    {
        public string North { get; set; } = string.Empty;
        public string East { get; set; } = string.Empty;
     
    }
}
