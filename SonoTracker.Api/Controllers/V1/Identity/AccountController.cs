using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SonoTracker.Api.Controllers.V1.Base;
using SonoTracker.Application.Services.Identity.Account;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Identity.User;
using SonoTracker.Domain.Entities.Identity;
using System.Net;

namespace SonoTracker.Api.Controllers.V1.Identity
{
    /// <summary>
    /// Controller for managing user accounts, including registration, login, and role updates.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AccountController"/> class.
    /// </remarks>
    /// <param name="accountService">Service for handling account-related operations.</param>
    /// <param name="userData">Data of the currently logged-in user.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService, UserData userData) : BaseController
    {

        /// <summary>
        /// Registers a new user with the provided details.
        /// </summary>
        /// <param name="user">The registration details of the user.</param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing a success message if the registration is successful, 
        /// or a conflict response if the registration fails.
        /// </returns>
        [HttpPost("register")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IFinalResult>> RegisterAsync([FromBody] RegisterDto user)
        {
            var loginResult = await accountService.RegisterAsync(user);

            if (loginResult?.Status == HttpStatusCode.BadRequest)
            {
                return BadRequest(loginResult);
            }
            else if(loginResult?.Status == HttpStatusCode.Conflict)
            {
                return Conflict(loginResult);
            }

            return Ok(loginResult);
        }

        /// <summary>
        /// Authenticates a user and generates an access token and refresh token.
        /// </summary>
        /// <param name="user">The login request containing the user's email and password.</param>
        /// <returns>
        /// An <see cref="IFinalResult"/> containing a <see cref="IFinalResult"/> if the login is successful, 
        /// or an unauthorized response if the login fails.
        /// </returns>
        [HttpPost("login")]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IFinalResult>> LoginAsync([FromBody] LoginRequestDto user)
        {
            var loginResult = await accountService.LoginAsync(user);

            // Fix: Cast the Data property to the appropriate type that contains the IsLogedIn property
            if (loginResult?.Data is LoginResponseDto loginData && loginData.IsLogedIn)
            {
                return Ok(loginResult);
            }

            return Unauthorized(loginResult);
        }

        /// <summary>
        /// Logs out the user with the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to log out.</param>
        /// <returns>An IActionResult indicating the result of the logout operation.</returns>
        [HttpPost("logout"), Authorize]
        public async Task<ActionResult<IFinalResult>> Logout(string userId)
        {
            var responseResult = new ResponseResult();

            User? user = await accountService.LogoutAsync(userId);

            if (user != null)
            {
                return Ok(responseResult.PostResult(user.Id, HttpStatusCode.OK,
                          message: HttpStatusCode.OK.ToString()));
            }

            return Unauthorized();
        }

        /// <summary>
        /// Refreshes the access token and refresh token for the user.
        /// </summary>
        /// <param name="model">The refresh token request containing user ID, access token, and refresh token.</param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing a <see cref="LoginResponseDto"/> if successful, 
        /// or an unauthorized response if the refresh operation fails.
        /// </returns>
        [HttpPost("refresh"), Authorize]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IFinalResult>> RefreshTokenAsync([FromBody] RefreshTokenRequestDto model)
        {
            var responseResult = new ResponseResult();

            var loginResult = await accountService.RefreshTokensAsync(model);

            if (loginResult?.IsLogedIn != true)
            {
                return Unauthorized(responseResult.PostResult(loginResult,
                                    HttpStatusCode.Unauthorized,
                                    message: HttpStatusCode.Unauthorized.ToString()));
            }

            return Ok(responseResult.PostResult(loginResult,
                      HttpStatusCode.OK,
                      message: HttpStatusCode.OK.ToString()));
        }

        /// <summary>
        /// Retrieves the data of the currently logged-in user from the access token.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> containing the <see cref="UserData"/> of the logged-in user.</returns>
        [HttpGet("tokenData"), Authorize]
        public ActionResult<IFinalResult> AccessTokenData()
        {
            var responseResult = new ResponseResult();

            return Ok(responseResult.PostResult(userData, HttpStatusCode.OK,
                      message: HttpStatusCode.OK.ToString()));
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing the <see cref="User"/> object if found, 
        /// or a not found response if the user does not exist.
        /// </returns>
        [HttpGet("get/{id}"), Authorize]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IFinalResult>> GetUserByIdAsync(string id)
        {
            var responseResult = new ResponseResult();

            var user = await accountService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(responseResult.PostResult(null, HttpStatusCode.NotFound,
                                message: "المستخدم غير موجود"));
            }

            return Ok(responseResult.PostResult(user, HttpStatusCode.OK,
                      message: HttpStatusCode.OK.ToString()));
        }
        /// <summary>
        /// Gets all users in the application.
        /// </summary>
        /// <returns>A list of users.</returns>
        [HttpGet("getAllUsers"), Authorize]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status404NotFound)]
        public ActionResult<IFinalResult> GetAllUsersAsync()
        {
            var responseResult = new ResponseResult();

            var users = accountService.GetUsersAsync();

            if (users == null || !users.Any())
            {
                return NotFound(responseResult.PostResult(null, HttpStatusCode.NotFound,
                                message: "لا يوجد مستخدمين"));
            }

            return Ok(responseResult.PostResult(users, HttpStatusCode.OK,
                      message: HttpStatusCode.OK.ToString()));
        }

        /// <summary>
        /// Retrieves a paginated list of users in the application.
        /// </summary>
        /// <returns>
        /// An <see cref="ActionResult"/> containing a paginated list of users if found, 
        /// or a not found response if no users exist.
        /// </returns>
        [HttpPost("getPaged"), Authorize]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IFinalResult>> GetAllPagedAsync(BaseParam<FilterUserDto> filter)
        {
            var users = await accountService.GetAllPagedAsync(filter);

            if (users == null || users.TotalCount == 0)
            {
                return NotFound("لا يوجد مستخدمين طبقا للبحث المطلوب");
            }

            return Ok(users);
        }

        /// <summary>
        /// Updates the personal data of the currently logged-in user.
        /// </summary>
        /// <param name="changeUserPersonalDataDto">The DTO containing the user's updated personal data.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the update operation. 
        /// Returns a success message if the update is successful, or a bad request response if the operation fails.
        /// </returns>
        [HttpPut("updateUserPersonalData"), Authorize]
        public async Task<ActionResult<IFinalResult>> UpdateUserPersonalData([FromBody] ChangeUserPersonalDataDto changeUserPersonalDataDto)
        {
            var responseResult = new ResponseResult();

            string userId = userData.Id!;

            var result = await accountService.UpdateUserPersonalData(userId, changeUserPersonalDataDto);

            if (!result.Succeeded)
                return BadRequest(responseResult.PostResult(null, 
                    HttpStatusCode.BadRequest,
                    message: "Failed to Update User Data : " +
                    string.Join(", ", result.Errors.Select(e => e.Description))));


            return Ok(responseResult.PostResult(result, HttpStatusCode.OK,
                      message: HttpStatusCode.OK.ToString()));

        }

        /// <summary>
        /// Updates the role of a user.
        /// </summary>
        /// <param name="user">The user role update request containing user ID and role ID.</param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing a success message if the role update is successful, 
        /// or a bad request response if the operation fails.
        /// </returns>
        [HttpPut("updateUserRole"), Authorize]
        //[ProducesResponseType<string>(StatusCodes.Status200OK)]
        //[ProducesResponseType<string>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IFinalResult>> UpdateUserRoleAsync([FromBody] UpdateUserRoleDto user)
        {
            var responseResult = new ResponseResult();

            var result = await accountService.UpdateUserRole(user.UserId, user.RoleId);

            if (result != "null")
            {
                return Ok(responseResult.PostResult(result, HttpStatusCode.OK,
                          message: HttpStatusCode.OK.ToString()));
            }

            return BadRequest(responseResult.PostResult(null, HttpStatusCode.BadRequest,
                              message: "المستخدم غير موجود أو خطأ فى تعديل الصلاحية"));
        }

        /// <summary>
        /// Removes a user by their unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// An <see cref="ActionResult"/> indicating whether the user was successfully removed.
        /// </returns>
        [HttpDelete("delete/{id}"), Authorize]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<IFinalResult>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IFinalResult>> RemoveUser(string id)
        {
            var responseResult = new ResponseResult();

            var res = await accountService.RemoveUser(id);

            if (!res)
            {
                return NotFound(responseResult.PostResult(res, HttpStatusCode.NotFound,
                                message: HttpStatusCode.NotFound.ToString()));
            }

            return Ok(responseResult.PostResult(res, HttpStatusCode.OK,
                      message: HttpStatusCode.OK.ToString()));
        }
    }
}
