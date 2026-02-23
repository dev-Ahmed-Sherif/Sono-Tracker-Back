using System.Threading.Tasks;

namespace SonoTracker.Common.Infrastructure.Repository.Company
{
    public interface ICompanyCustomRepository : IRepository<Domain.Entities.Company>
    {
        Task<Domain.Entities.Company> AddCompanyAsync(Domain.Entities.Company company);
    }
}
