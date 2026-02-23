using SonoTracker.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonoTracker.Common.DTO.Tracker.Accident.Parameters
{
    [ExcludeFromCodeCoverage]
   public class AccidentFilter
    {
        public int? Number { get; set; }
        public DateTime? AccidentDate { get; set; }
        public Guid? TownId { get; set; }
        public Guid? AccidentTypeId { get; set; }
        public Case? CaseId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? FloatingUnitId { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}
