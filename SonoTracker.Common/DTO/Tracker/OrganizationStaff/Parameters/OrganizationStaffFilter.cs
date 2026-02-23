using SonoTracker.Domain.Enum;
using System;

namespace SonoTracker.Common.DTO.Tracker.OrganizationStaff.Parameters
{
    public class OrganizationStaffFilter
    {
        public string Name { get; set; } = string.Empty;

        public string Job { get; set; } = string.Empty;

        public Gender? Gender { get; set; }

        public IDType? IDType { get; set; }

        public string Identity { get; set; } = string.Empty;

        public Guid? NationalityId { get; set; }

        public Guid? OrganizationId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsDelegate { get; set; } = false;

    }
}
