using Microsoft.AspNetCore.Identity;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Identity.User;
using SonoTracker.Common.DTO.Tracker.LicenseApplication.Parameters;
using SonoTracker.Domain.Entities.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Identity.Account
{
    public interface IAccountService
    {
        Task<IFinalResult> RegisterAsync(RegisterDto request);
        Task<IFinalResult> LoginAsync(LoginRequestDto request);
        Task<LoginResponseDto> RefreshTokensAsync(RefreshTokenRequestDto request);
        Task<IdentityResult> UpdateUserPersonalData(string userId, ChangeUserPersonalDataDto changePersonalData);
        Task<User> LogoutAsync(string id);
        Task<string> UpdateUserRole(string userId, string roleId);
        Task<UserDto> GetUserByIdAsync(string userId);
        IEnumerable<UserDto> GetUsersAsync();
        Task<PagingResult> GetAllPagedAsync(BaseParam<FilterUserDto> filter);
        Task<bool> RemoveUser(string userId);
    }
}
