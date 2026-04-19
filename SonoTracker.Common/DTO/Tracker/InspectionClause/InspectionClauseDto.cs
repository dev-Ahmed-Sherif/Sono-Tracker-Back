using SonoTracker.Common.Core;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Common.DTO.Tracker.InspectionClause
{
    [ExcludeFromCodeCoverage]
    public class InspectionClauseDto : IEntityDto<string>
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public string? ParentName { get; set; }
        public string? InspectionTypeId { get; set; }
        public string? InspectionTypeName { get; set; }
    }
}
