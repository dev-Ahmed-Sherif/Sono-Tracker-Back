using System.Threading.Tasks;
using SonoTracker.Common.DTO.Hr.Employee;

namespace SonoTracker.Integration.CacheRepository
{
    public interface ICacheRepository
    {
        /// <summary>
        /// Get Employee From Cache By National Id
        /// </summary>
        /// <param name="nationalId"></param>
        /// <returns></returns>
        Task<EmployeeProfileDto> GetEmployeeAsync(string nationalId);
    }
}
