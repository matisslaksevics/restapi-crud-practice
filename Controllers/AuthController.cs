using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Dtos.Auth;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Services.SAuth;


namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        // POST /auth/register
        [HttpPost("register")]
        public async Task<ActionResult<Client>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user is null)
                return BadRequest("Username already exists.");

            return Ok(user);
        }
        // POST /auth/login
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var result = await authService.LoginAsync(request);
            if (result is null)
                return BadRequest("Invalid username or password.");

            return Ok(result);
        }
        // POST /auth/refresh-token
        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var clientId = UserHelper.GetUserId(User);
            if (clientId == null) return Unauthorized("Could not determine user from token.");

            var result = await authService.RefreshTokensAsync(clientId, request.RefreshToken);
            if (result is null)
                return Unauthorized("Invalid refresh token.");

            return Ok(result);
        }

        // GET /auth/authenticated-only
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        // GET /auth/admin-only
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        [ProducesResponseType(typeof(bool),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are and admin!");
        }

        // POST /auth/check-password
        [Authorize]
        [HttpPost("check-password")]
        public async Task<IActionResult> CheckPassword()
        {
            var clientId = UserHelper.GetUserId(User);
            if (clientId == null) return Unauthorized("Could not determine user from token.");

            var status = await authService.CheckPasswordAsync(clientId);
            if (status is null) return NotFound();

            return Ok(status);
        }

        // POST /auth/change-password
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto body)
        {
            var clientId = UserHelper.GetUserId(User);
            if (clientId == null) return Unauthorized("Could not determine user from token.");

            var changed = await authService.ChangePasswordAsync(clientId, body.CurrentPassword, body.NewPassword);
            if (!changed) return BadRequest("Current password is incorrect.");

            return NoContent();
        }

        // POST /auth/signout
        [Authorize]
        [HttpPost("signout")]
        public async Task<IActionResult> NewSignOut()
        {
            var clientId = UserHelper.GetUserId(User);
            if (clientId == null) return Unauthorized("Could not determine user from token.");

            await authService.SignOutAsync(clientId);
            return NoContent();
        }

        // POST /auth/profile
        [Authorize]
        [HttpPost("profile")]
        public async Task<ActionResult<UserProfileDto>> CheckProfile()
        {
            var clientId = UserHelper.GetUserId(User);
            if (clientId == null) return Unauthorized("Could not determine user from token.");

            var result = await authService.GetProfileAsync(clientId);
            return Ok(result);
        }

        // POST /auth/admin/profile/{userId}
        [Authorize(Roles = "Admin")]
        [HttpPost("admin/profile/{userId:guid}")]
        public async Task<ActionResult<UserProfileDto>> CheckProfile(Guid userId)
        {
            var result = await authService.GetProfileAsync(userId);
            return result is null ? NotFound() : Ok(result);
        }

        // POST /auth/admin/change-password
        [Authorize(Roles = "Admin")]
        [HttpPost("admin/change-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] AdminPasswordChangeDto body)
        {
            var changed = await authService.AdminSetPasswordAsync(body.Id, body.NewPassword);
            if (!changed) return BadRequest("Current password is incorrect.");

            return NoContent();
        }

        // POST /auth/admin/change-role
        [Authorize(Roles="Admin")]
        [HttpPost("admin/change-role")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleDto body)
        {
            var changed = await authService.ChangeUserRoleAsync(body.Id, body.NewRole);
            if (!changed) return BadRequest("Could not change user role.");
            return NoContent();
        }
    }
}
