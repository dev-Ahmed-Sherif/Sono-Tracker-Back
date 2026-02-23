using System;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;

namespace SonoTracker.Common.DTO.Tracker.Organization
{
    [ExcludeFromCodeCoverage]
    public class OrganizationDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Guid NationalityId { get; set; }
        public string NationalityName { get; set; }
        public OrganizationType OrganizationTypeId { get; set; }
        public EnumResult OrganizationType { get; set; }
        public AppliedOn AppliedOn { get; set; }
        public Guid InspectionTypeId { get; set; }
        public string InspectionType { get; set; }
        public DateTime CreationDate { get; set; }
        public string CommercialRegistrationNumber { get; set; }
        public string WebSiteAddress { get; set; }
        public int TouristMarinaNumber { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsReport { get; set; }
    }
}
