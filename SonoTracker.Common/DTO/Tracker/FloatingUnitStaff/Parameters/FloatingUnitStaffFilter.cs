using SonoTracker.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.FloatingUnitStaff.Parameters
{
    [ExcludeFromCodeCoverage]

    public class FloatingUnitStaffFilter
    {
        public string Name { get; set; }
     
        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public Gender? gender { get; set; }

        public IDType? IDType { get; set; }

        public string Identity { get; set; }

        public Guid? FloatingUnitId { get; set; }

        public Guid? NationalityId { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsDelegate { get; set; } 

    }
}
