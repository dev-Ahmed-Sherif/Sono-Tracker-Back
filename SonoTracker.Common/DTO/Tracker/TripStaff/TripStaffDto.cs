using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripStaff
{
    [ExcludeFromCodeCoverage]
    public class TripStaffDto : IEntityDto<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public IDType IDType { get; set; }
        public string Identity { get; set; }
        public string NationalityId { get; set; }
        public string NationalityNameAr { get; set; }
        public string NationalityNameEn { get; set; }
        public string TripInformationId { get; set; }
        public string TripInformationCode { get; set; }
        public string GovernorateId { get; set; }
        public string GovernorateNameAr { get; set; }
        public string GovernorateNameEn { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedById { get; set; }
    }
}
