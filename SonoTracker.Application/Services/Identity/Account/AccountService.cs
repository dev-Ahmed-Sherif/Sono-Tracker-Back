using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SonoTracker.Common.Constants.Auth;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Identity.User;
using SonoTracker.Common.Infrastructure.UnitOfWork;
using SonoTracker.Domain;
using SonoTracker.Domain.Entities.Identity;
using SonoTracker.Infrastructure.Context;

namespace SonoTracker.Application.Services.Identity.Account
{

    public class AccountService(
                 UserManager<User> userManager,
                 RoleManager<IdentityRole> roleManager,
                 SonoTrackerDbContext context,
                 UserData auditUser,
                 IConfiguration configuration,
                 SignInManager<User> signInManager,
                 IUnitOfWork<User> UnitOfWork,
                 IMapper Mapper) : IAccountService
    {
        public async Task<IFinalResult> RegisterAsync(RegisterDto request)
        {
            var responseResult = new ResponseResult();
            User checkUser = await userManager.FindByEmailAsync(request.Email);
            if (checkUser == null)
            {
                User user = new()
                {
                    Email = request.Email,
                    UserName = request.Email,
                    FullName = request.Username,
                    CreatedById = auditUser.Id != "" ? auditUser.Id : "",
                    CreatedAt = DateTime.Now,
                    ModifiedById = auditUser.Id != "" ? auditUser.Id : "",
                    ModifiedAt = DateTime.Now,
                };

                IdentityResult result = await userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    return responseResult.PostResult(user, status: HttpStatusCode.BadRequest,
                        message: "Failed to Create User : " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));

                user.CreatedById = auditUser.Name != "" ? auditUser.Name : user.FullName;
                user.CreatedAt = DateTime.Now;
                user.ModifiedById = auditUser.Name != "" ? auditUser.Name : user.FullName;
                user.ModifiedAt = DateTime.Now;
                

                await userManager.UpdateAsync(user);

                if (!string.IsNullOrWhiteSpace(request.RoleId))
                {
                    IdentityRole role = await roleManager.FindByIdAsync(request.RoleId);

                    if (role == null)
                    {
                        //return "null";
                        IdentityRole userRole = await roleManager.FindByNameAsync(Roles.User);

                        await userManager.AddToRoleAsync(user, userRole.Name!);

                        return responseResult.PostResult(user.Id, status: HttpStatusCode.Accepted,
                                message: HttpStatusCode.OK.ToString());
                    }

                    await userManager.AddToRoleAsync(user, role.Name!);

                    return responseResult.PostResult(user.Id, status: HttpStatusCode.OK,
                        message: HttpStatusCode.OK.ToString());
                }
                else
                {
                    IdentityRole role = await roleManager.FindByNameAsync(Roles.User);
                  
                    await userManager.AddToRoleAsync(user, role.Name!);
                    
                    return  responseResult.PostResult(user.Id, status: HttpStatusCode.Accepted,
                            message: HttpStatusCode.OK.ToString()); 
                }

            }

            return responseResult.PostResult(null,status: HttpStatusCode.Conflict,
                   message: "User Already Exist");
        }
        public async Task<IFinalResult> LoginAsync(LoginRequestDto request)
        {
            LoginResponseDto response = new();
            var responseResult = new ResponseResult();
            User user = await userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            }
            else
            {
                return responseResult.PostResult(response, status: HttpStatusCode.Unauthorized,
                    message: "Invalid email or password");
            }
           

            user!.IsLogedIn = true;
            string userName = user.FullName;
            user.ModifiedById = userName;
            user.ModifiedAt = DateTime.Now;

            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                //return response;
                // Create an instance of ResponseResult to call the PostResult method
                return responseResult.PostResult(response, status: HttpStatusCode.Unauthorized,
                     message: "Invalid email or password");
            }

            await userManager.UpdateAsync(user);

            response = await CreateTokenResponse(user);

            //return response;
            return responseResult.PostResult(response, status: HttpStatusCode.OK,
                    message: HttpStatusCode.OK.ToString());
        }
        private async Task<LoginResponseDto> CreateTokenResponse(User user)
        {
            IList<Claim> perUserClaim = await userManager.GetClaimsAsync(user);

            IList<string> userRole = await userManager.GetRolesAsync(user);

            IList<Claim> perRoleClaim = [];

            for (int i = 0; i < userRole.Count; i++)
            {
                IdentityRole roleName = await roleManager.FindByNameAsync(userRole[i]);
                perRoleClaim = await roleManager.GetClaimsAsync(roleName!);
            }

            IEnumerable<Claim> perUser = perUserClaim.Union(perRoleClaim);

            return new LoginResponseDto
            {
                IsLogedIn = true,
                AccessToken = await CreateToken(user, perUser),
                RefreshToken = await GenerateAndSaveRefreshTokensAsync(user)
            };
        }
        public async Task<LoginResponseDto> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null) return null;

            return await CreateTokenResponse(user);
        }
        private async Task<User> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            User user = await userManager.FindByIdAsync(userId);

            var token = await context.RefreshTokens
                        .Where(x => x.Token == refreshToken &&
                               x.User.Id == userId &&
                               x.ExpiryTime >= DateTime.Now)
                        .FirstOrDefaultAsync();

            await RemoveOldRefreshToken(userId, refreshToken);


            if (user is null || token is null)
            {
                return null;
            }

            return user;
        }
        private async Task<RefreshToken> RemoveOldRefreshToken(string userId, string refreshToken)
        {
            var OldToken = await context.RefreshTokens
                   .Where(x => x.Token == refreshToken && x.User.Id == userId)
                   .FirstOrDefaultAsync();

            if (OldToken is not null)
            {
                context.RefreshTokens.Remove(OldToken!);
                await context.SaveChangesAsync();
            }
            return OldToken;
        }
        private async Task<string> GenerateAndSaveRefreshTokensAsync(User user)
        {
            // Generate refresh token
            string refreshToken = GenerateRefreshToken();
            // Save refresh token
            RefreshToken token = new()
            {
                Token = refreshToken,
                UserId = user.Id,
                User = user,
                ExpiryTime = DateTime.Now.AddDays(AuthConstants.RefreshTokenLife),
                CreatedBy = user.FullName,
                CreatedById = user.Id,
                CreatedDate = DateTime.Now,
                ModifiedBy = user.FullName,
                ModifiedById = user.Id,
                ModifiedDate = DateTime.Now,
                IsDeleted = false,
                IpAddress = ""
            };

            await context.RefreshTokens.AddAsync(token);
            await context.SaveChangesAsync();

            return token.Token;
        }
        private static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private async Task<string> CreateToken(User user, IEnumerable<Claim> claimDB)
        {
            IList<string> userRoles = await userManager.GetRolesAsync(user);
            IEnumerable<Claim> roles = userRoles.Select(o => new Claim(ClaimTypes.Role, o));
            //IEnumerable<Claim> claims = new[]
            //{
            //    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            //    new Claim(ClaimTypes.Name,user.FullName),
            //    new Claim(AuthConstants.OrgId, user.Organization.Id.ToString()),
            //    new Claim(ClaimTypes.Role, roles.FirstOrDefault().ToString()),

            //}.Union(roles).Union(claimDB);
            var role = userRoles.FirstOrDefault() ?? Roles.User; // Default to User role if no roles assigned
            IEnumerable<Claim> claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.FullName),
                new Claim(ClaimTypes.Role, role),
                new Claim(AuthConstants.OrgId, user.OrganizationId != null ? user.OrganizationId.ToString() : ""),
                new Claim(AuthConstants.FloatingUnitId, user.FloatingUnitId != null ? user.FloatingUnitId.ToString() : ""),

            }.Union(claimDB);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")!));


            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: configuration.GetValue<string>("Jwt:Issuer"),
                claims: claims,
                expires: DateTime.Now.AddHours(AuthConstants.AccessTokenLifeInHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public async Task<IdentityResult> UpdateUserPersonalData(string userId, ChangeUserPersonalDataDto changePersonalData)
        {
            User user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return IdentityResult.Failed(new IdentityError() { Description = "User Not Found" });

            user.Email = changePersonalData.Email;
            user.UserName = changePersonalData.Email;
            user.FullName = changePersonalData.UserName;
            user.ModifiedById = auditUser.Id != "" ? auditUser.Id : user.Id;
            user.ModifiedAt = DateTime.Now;

            await userManager.UpdateAsync(user);
            return await userManager.ChangePasswordAsync(user, changePersonalData.OldPassword, changePersonalData.NewPassword);
        }
        public async Task<User> LogoutAsync(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user is not null)
            {
                RefreshToken refreshToken = await context.RefreshTokens.Where(rf => rf.User.Id == user.Id).FirstOrDefaultAsync();
                if (refreshToken is not null)
                {
                    await RemoveOldRefreshToken(id, refreshToken!.Token);
                    user.IsLogedIn = false;
                    await userManager.UpdateAsync(user);
                    //await signInManager.SignOutAsync();
                    return user;
                }
            }
            return null;

        }
        public async Task<string> UpdateUserRole(string userId, string roleId)
        {
            User user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
            {
                return "null";
            }

            IdentityRole role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return "null";
            }

            IList<string> userRole = await userManager.GetRolesAsync(user);
            if (userRole.Count > 0)
            {
                await userManager.RemoveFromRolesAsync(user, userRole);
            }

            await userManager.AddToRoleAsync(user, role.Name!);
            return "OK";

        }
        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            User user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return null;
            }
            UserDto userDto = new()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? "",
                FloatingUnitId = user.FloatingUnitId.ToString(),
                OrganizationId = user.OrganizationId.ToString(),
            };
            return userDto;
        }
        public async Task<bool> RemoveUser(string userId)
        {
            User user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
            {
                return false;
            }
            IdentityResult Result = await userManager.DeleteAsync(user);
            return Result.Succeeded ? true : false;
        }

        public IEnumerable<UserDto> GetUsersAsync()
        {
            var users = userManager.Users.Select(u => new UserDto 
            { 
                Id=u.Id,
                Email=u.Email,
                FullName = u.FullName,
                Role = userManager.GetRolesAsync(u).Result.FirstOrDefault() ?? "",
                FloatingUnitId = u.FloatingUnitId.ToString(),
                OrganizationId = u.OrganizationId.ToString()
            });
            return users;
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<FilterUserDto> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var (Count, Result) = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue);
            
            var data = Mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(Result);

            for (int i = 0; i < data.ToList().Count; i++)
            {
                var email = data.ToList()[i].Email;
                var user = await userManager.FindByEmailAsync(email);
                IList<string> userRoles = await userManager.GetRolesAsync(user);
                data.ToList()[i].Role = userRoles.FirstOrDefault() ?? ""; // Default to User role if no roles assigned
            }

            return new PagingResult(filter.PageNumber, filter.PageSize, Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }
        static Expression<Func<User, bool>> PredicateBuilderFunction(FilterUserDto filter)
        {
            var predicate = PredicateBuilder.New<User>(true);
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                predicate = predicate.And(x => x.FullName.Contains(filter.Name));
            }
            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                predicate = predicate.And(x => x.Email.Equals(filter.Email));
            }


            return predicate;
        }
    }
}
