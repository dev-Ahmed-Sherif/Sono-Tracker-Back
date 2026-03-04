using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.FloatingUnit
{
    [ExcludeFromCodeCoverage]

    public class AddFloatingUnitDto : IEntityDto<string>
    {
        public string Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string Code { get; set; }

        public string LicenseNumber { get; set; }

        public float Length { get; set; }

        public float Width { get; set; }

        public int PassengerNumber { get; set; }

        public int RoomNumber { get; set; }

        public string ManufactureYear { get; set; }

        public DateTime? LastMaintenanceDate { get; set; }

        public DateTime? NextMaintenanceDate { get; set; }

        public UnitCategory unitCategory { get; set; }

        public string UnitTypeId { get; set; }
    }
}
