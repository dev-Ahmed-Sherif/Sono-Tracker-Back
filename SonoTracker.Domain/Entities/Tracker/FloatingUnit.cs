using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class FloatingUnit : Lookup<string>
    {
        public FloatingUnit()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [Required]
        public required string LicenseNumber { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public int PassengerNumber { get; set; }
        public int RoomNumber { get; set; }
        public DateOnly ManufactureYear { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }

        [MaxLength(50), ForeignKey(nameof(UnitType))]
        public string? UnitTypeId { get; set; }
        public virtual UnitType? UnitType { get; set; }

        [MaxLength(50), ForeignKey(nameof(Governorate))]
        public string? GovernorateId { get; set; }
        public virtual Governorate? Governorate { get; set; }

        public virtual HashSet<FloatingUnitStaff> FloatingUnitStaffs { get; set; } = [];
        public virtual HashSet<FloatingUnitOrganization> FloatingUnitOrganizations { get; set; } = [];
        public virtual HashSet<TripInformation> TripInformations { get; set; } = [];
        public virtual HashSet<Accident> Accidents { get; set; } = [];
        public virtual HashSet<Maintenance> Maintenances { get; set; } = [];
        public virtual HashSet<Inspection> Inspections { get; set; } = [];
    }
}
