using SonoTracker.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.LicenseApplication.Parameters
{
    [ExcludeFromCodeCoverage]

    public class LicenseApplicationFilter
    {
        public string LicenseNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? FromOrganizationId { get; set; }
        public Guid? ToOrganizationId { get; set; }
        public Status? Status { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
