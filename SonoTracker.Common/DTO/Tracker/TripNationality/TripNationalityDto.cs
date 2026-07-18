using SonoTracker.Common.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripNationality
{
    [ExcludeFromCodeCoverage]
    public class TripNationalityDto : IEntityDto<string>
    {
        public string Id { get; set; }
        public string NationalityId { get; set; }
        public string NationalityNameAr { get; set; }
        public string NationalityNameEn { get; set; }
        public string NationalityCode { get; set; }
        public string TripInformationId { get; set; }
        public string TripInformationCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedById { get; set; }
    }
}
