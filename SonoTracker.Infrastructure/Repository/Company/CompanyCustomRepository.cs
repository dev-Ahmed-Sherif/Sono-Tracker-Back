using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Common.Infrastructure.Repository.Company;

namespace SonoTracker.Infrastructure.Repository.Company
{
    public class CompanyCustomRepository : Repository<Domain.Entities.Company>, ICompanyCustomRepository
    {
        private readonly DbContext _dbContext;
        public CompanyCustomRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Domain.Entities.Company> AddCompanyAsync(Domain.Entities.Company company)
        {
            var result = await DbSet.AddAsync(company);
            return result.Entity;
        }
        
    }
}
