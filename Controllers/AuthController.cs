using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Dtos.Auth;
using restapi_crud_practice.Dtos.Token;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Services.SAuth;
using restapi_crud_practice.Services.SUserContext;

namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, IUserContextService userContext) : ControllerBase
    {

        // POST /auth/register
        [HttpPost("register")]
        public async Task<ActionResult<Client>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user is null)
            {
                return BadRequest("Username already exists.");
            }

            return Ok(user);

        }
        // POST /auth/login
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var result = await authService.LoginAsync(request);
            if (result is null)
            {
                return BadRequest("Invalid username or password.");
            }

            return Ok(result);
        }
        // POST /auth/refresh-token
        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var clientId = userContext.GetUserId(User);
            if (clientId == null)
            {
                return Unauthorized("Could not determine user from token.");
            }

            var (tokens, error) = await authService.RefreshTokensAsync(clientId.Value, request.RefreshToken);

            if (error != null)
            {
                return Unauthorized(error);
            }

            return Ok(tokens);
        }

        // POST /auth/check-password
        [Authorize]
        [HttpPost("check-password")]
        public async Task<IActionResult> CheckPassword()
        {
            var clientId = userContext.GetUserId(User);

            var status = await authService.CheckPasswordAsync(clientId);
            if (status is null)
            {
                return NotFound();
            }
            else {
                return Ok(status);
            }
        }

        // POST /auth/change-password
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto body)
        {
            var clientId = userContext.GetUserId(User);

            var changed = await authService.UpdatePasswordAsync(clientId, body.CurrentPassword, body.NewPassword);
            if (!changed)
            {
                return BadRequest("Current password is incorrect or user is not found.");
            }
            else {
                return NoContent();
            }
        }

        // POST /auth/signout
        [Authorize]
        [HttpPost("signout")]
        public async Task<IActionResult> NewSignOut()
        {
            var clientId = userContext.GetUserId(User);

            var result = await authService.SignOutAsync(clientId);
            if (!result)
            {
                return BadRequest("Could not sign out user.");
            }
            return NoContent();
        }

        // POST /auth/profile
        [Authorize]
        [HttpPost("profile")]
        public async Task<ActionResult<UserProfileDto>> CheckProfile()
        {
            var clientId = userContext.GetUserId(User);

            var result = await authService.GetProfileAsync(clientId);
            if (result is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }
        // POST /auth/admin/profile/{userId}
        [Authorize(Roles = "Admin")]
        [HttpPost("admin/profile/{userId:guid}")]
        public async Task<ActionResult<UserProfileDto>> CheckProfile(Guid userId)
        {
            var result = await authService.GetProfileAsync(userId);
            if (result is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        // POST /auth/admin/change-password
        [Authorize(Roles = "Admin")]
        [HttpPost("admin/change-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] AdminPasswordChangeDto body)
        {
            var changed = await authService.AdminUpdatePasswordAsync(body.Id, body.NewPassword);
            if (!changed)
            {
                return BadRequest("Current password is incorrect.");
            } 

            return NoContent();
        }

        // POST /auth/admin/change-role
        [Authorize(Roles="Admin")]
        [HttpPost("admin/change-role")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleDto body)
        {
            var changed = await authService.UpdateUserRoleAsync(body.Id, body.NewRole);
            if (!changed)
            {
                return BadRequest("Could not change user role.");
            } 

            return NoContent();
        }
    }
}
