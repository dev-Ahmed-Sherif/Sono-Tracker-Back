using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Lookups;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class NationalityTrip : BaseEntity<string>
    {
        public NationalityTrip()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [MaxLength(50), ForeignKey(nameof(Nationality))]
        public string? NationalityId { get; set; }
        public virtual Nationality? Nationality { get; set; }

        [MaxLength(50), ForeignKey(nameof(TripInformation))]
        public string? TripInformationId { get; set; }
        public virtual TripInformation? TripInformation { get; set; }
        
        [MaxLength(50), ForeignKey(nameof(Governorate))]
        public string? GovernorateId { get; set; }
        public virtual Governorate? Governorate { get; set; }
        public int NationalityNumber { get; set; }
    }
}
