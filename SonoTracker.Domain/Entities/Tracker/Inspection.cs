using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Enum;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class Inspection : BaseEntity<string>
    {
        public Inspection()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        public DateTime InspectionDate { get; set; }

        [MaxLength(50), ForeignKey(nameof(TripInformation))]
        public string? TripInformationId { get; set; }
        public virtual TripInformation? TripInformation { get; set; }

        [MaxLength(50), ForeignKey(nameof(FloatingUnit))]
        public string? FloatingUnitId { get; set; }
        public virtual FloatingUnit? FloatingUnit { get; set; }

        [Required]
        [MaxLength(50), ForeignKey(nameof(Organization))]
        public required string OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }
        public InspectionType InspectionTypeId { get; set; }
        public bool IsInspected { get; set; }
        public bool SaftyPetroleumWaste { get; set; }
        public bool RightWasteDisposal { get; set; }
        public string? Note { get; set; }
        public string? InspectionAttachment { get; set; }
    }
}
