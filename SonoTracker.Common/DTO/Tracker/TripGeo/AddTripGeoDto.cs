using SonoTracker.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.TripGeo
{
    public class AddTripGeoDto : IEntityDto<Guid?>
    {
        public Guid? Id { get ; set; }
        public Guid GeoPointId { get; set; }
        public Guid TripInformationId { get; set; }   

    }
}
