using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Tracker.TripAttachment;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripInformation
{
    [ExcludeFromCodeCoverage]
    public class EditTripInformationDto : IEntityDto<string>
    {
        public string Id { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public string Code { get; set; }

        public string FloatingUnitId { get; set; }

        public string FloatingUnitNameAr { get; set; }

        public string FloatingUnitNameEn { get; set; }

        public string FloatingUnitCode { get; set; }

        public int StaffNumber { get; set; }

        public int PassengerNumber { get; set; }

        public int AttachmentNumber { get; set; }

        public bool IsAccepted { get; set; }

        public string PassengerAttachment { get; set; }

        public string StaffAttachment { get; set; }

        public string RouteId { get; set; }

        public string RouteName { get; set; }

        public string GovernorateId { get; set; }

        public string GovernorateNameAr { get; set; }

        public ICollection<TripAttachmentDto> TripAttachments { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedById { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime ModifiedAt { get; set; }

        public string ModifiedBy { get; set; }

        public string ModifiedById { get; set; }
    }
}
