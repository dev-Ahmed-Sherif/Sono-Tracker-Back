using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SonoTracker.Common.Core;
using SonoTracker.Domain.Enum;

namespace SonoTracker.Common.DTO.Tracker.Inspection
{
   public class EditGeneralInspectionDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }

        public DateTime InspectionDate { get; set; }

        public Guid? TripInformationId { get; set; }
        public Guid? FloatingUnitId { get; set; }

        public Guid OrganizationId { get; set; }

        public InspectionType InspectionType { get; set; }

        public bool IsInspected { get; set; }

        public bool SaftyPetroleumWaste { get; set; }

        public bool RightWasteDisposal { get; set; }

        public string Note { get; set; }

        public string InspectionAttachment { get; set; }

    }
}
