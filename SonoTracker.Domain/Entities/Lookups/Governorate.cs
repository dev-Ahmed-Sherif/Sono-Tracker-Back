using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Tracker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Domain.Entities.Lookups
{
    [ExcludeFromCodeCoverage]
    public class Governorate : BaseAudit<string>
    {
        public Governorate()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.CreateVersion7().ToString();
            }
        }

        [Required, MaxLength(100)]
        public required string Name { get; set; }
        
        [Required, MaxLength(10)]
        public required string Code { get; set; }

        [Required, MaxLength(100)]
        public required string WebsiteUrl { get; set; }

        [Required, MaxLength(250)]
        public required string Address { get; set; }

        [Required, MaxLength(250)]
        public required string ImageUrl { get; set; }
       
        public virtual HashSet<Town> Cities { get; set; } = [];

    }
}
