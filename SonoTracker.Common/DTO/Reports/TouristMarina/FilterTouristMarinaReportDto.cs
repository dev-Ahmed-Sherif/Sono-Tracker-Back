using SonoTracker.Domain.Entities.Tracker;
using System.ComponentModel.DataAnnotations;
using System;
using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Common.DTO.ReportsDTOs;

namespace SonoTracker.Common.DTO.Reports.TouristMarina
{
    public class FilterTouristMarinaReportDto : BaseReportSearch
    {
        public Guid? TouristMarinaId { get; set; }

        public Guid? OrganizationId { get; set; }
        public Guid? TownId { get; set; }
         public bool IsDeleted { get; set; } = false;

    }
}
