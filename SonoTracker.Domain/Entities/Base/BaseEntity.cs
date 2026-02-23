using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Base
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
        [MaxLength(100)]
        public string CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
        [MaxLength(100)]
        public string ModifiedById { get; set; }
        public bool IsDeleted { get; set; } = false;
        [MaxLength(250)]
        public string? CreatedByEmployeeEn { get; set; }
        [MaxLength(250)]
        public string? CreatedByEmployeeAr { get; set; }
        [MaxLength(250)]
        public string? ModifiedByEmployeeEn { get; set; }
        [MaxLength(250)]
        public string? ModifiedByEmployeeAr { get; set; }
        [MaxLength(100)]
        public string? CreatedByEmployeeId { get; set; }
        [MaxLength(100)]
        public string? ModifiedByEmployeeId { get; set; }
        [MaxLength(100)]
        public string? IpAddress { get; set; }
    }
}