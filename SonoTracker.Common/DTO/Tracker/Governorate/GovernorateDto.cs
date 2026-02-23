using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Common.Core;

namespace SonoTracker.Common.DTO.Tracker.Governorate
{
    [ExcludeFromCodeCoverage]
    public class GovernorateDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
    }
}
