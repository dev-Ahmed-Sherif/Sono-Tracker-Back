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
        Task<IFinalResult> LogoutAsync(string id);
        Task<LoginResponseDto> RefreshTokensAsync(RefreshTokenRequestDto request);
        Task<IFinalResult> GetUserByIdAsync(string userId);
        Task<IFinalResult> GetUsersAsync();
        Task<PagingResult> GetAllPagedAsync(BaseParam<FilterUserDto> filter);
        Task<IFinalResult> UpdateUser(UpdateUserDto changePersonalData);
        Task<IFinalResult> DeleteUser(string userId);
    }
}
