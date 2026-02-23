using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Tracker
{
    [ExcludeFromCodeCoverage]
    public class FloatingUnit : BaseEntity<Guid>
    {
        [MaxLength(100)]
        public string NameAr { get; set; }

        [MaxLength(100)]
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string LicenseNumber { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public int PassengerNumber { get; set; }
        public int RoomNumber { get; set; }

        [MaxLength(100)]
        public string ManufactureYear { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public UnitCategory UnitCategory { get; set; }
        public Guid UnitTypeId { get; set; }
        public virtual UnitType UnitType { get; set; }
        public virtual ICollection<FloatingUnitStaff> FloatingUnitStaffs { get; set; } = new HashSet<FloatingUnitStaff>();
        public virtual ICollection<FloatingUnitOrganization> FloatingUnitOrganizations { get; set; } = new HashSet<FloatingUnitOrganization>();
        public virtual ICollection<TripInformation> TripInformations { get; set; } = new HashSet<TripInformation>();
        public virtual ICollection<Accident> Accidents { get; set; } = new HashSet<Accident>();
        public virtual ICollection<Maintenance> Maintenances { get; set; } = new HashSet<Maintenance>();
        public virtual ICollection<Inspection> Inspections { get; set; } = new HashSet<Inspection>();
    }
}
