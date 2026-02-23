using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.MarinaTrip
{

    [ExcludeFromCodeCoverage]
    public class EditMarinaTripDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }

        public Guid TouristMarinaId { get; set; }

        public Guid TripInformationId { get; set; }

    }
}
