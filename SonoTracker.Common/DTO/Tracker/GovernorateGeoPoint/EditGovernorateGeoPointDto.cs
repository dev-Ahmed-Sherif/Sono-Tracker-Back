using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Common.Core;

namespace SonoTracker.Common.DTO.Tracker.GovernorateGeoPoint
{
    [ExcludeFromCodeCoverage]
    public class EditGovernorateGeoPointDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public Guid GeoPointId { get; set; }

        public Guid GovernorateId { get; set; }
    }
}
