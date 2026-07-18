using Microsoft.AspNetCore.Http;
using SonoTracker.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.TripInformation
{
    [ExcludeFromCodeCoverage]
    public class AddTripInformationDto : IEntityDto<string>
    {
        public string? Id { get; set; }

        [Required]
        public required DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public string Code { get; set; }

        [Required, MaxLength(50)]
        public required string FloatingUnitId { get; set; }

        [Required]
        public int StaffNumber { get; set; }

        [Required]
        public int PassengerNumber { get; set; }

        [Required]
        public int AttachmentNumber { get; set; }

        [Required]
        public required bool IsAccepted { get; set; }

        [Required, MaxLength(50)]
        public required string RouteId { get; set; }

        [Required]
        public required IFormFile PassengerAttachment { get; set; }

        [Required]
        public required IFormFile StaffAttachment { get; set; }

        public List<IFormFile> OtherAttachment { get; set; }
    }
}
