using SonoTracker.Common.DTO.Company;
using SonoTracker.Domain.Entities;

// ReSharper disable once CheckNamespace
namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapCompany()
        {
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<Company, EditCompanyDto>().ReverseMap();
            CreateMap<Company, AddCompanyDto>().ReverseMap();
        }
    }
}