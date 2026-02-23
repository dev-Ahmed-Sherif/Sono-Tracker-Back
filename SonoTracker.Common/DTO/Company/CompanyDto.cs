using System;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.Core;

namespace SonoTracker.Common.DTO.Company
{
    [ExcludeFromCodeCoverage]
    public  class CompanyDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }

        public string NameEn { get; set; }

        public string NameAr { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
