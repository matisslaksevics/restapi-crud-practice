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
    public class AuthController(IAuthService authService, IUserContextService userContext, ILogger<AuthController> logger) : ControllerBase
    {
        // POST /auth/register
        [HttpPost("register")]
        public async Task<ActionResult<Client>> Register(UserDto request)
        {
            logger.LogInformation(
                "POST Register requested from IP {RemoteIp}",
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                var user = await authService.RegisterAsync(request);
                if (user is null)
                {
                    return BadRequest("Username already exists.");
                }
                else
                {
                    logger.LogInformation("POST Register successful.");
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST Register failed.");
                throw;
            }
        }
        // POST /auth/login
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            logger.LogInformation(
                "POST Login requested from IP {RemoteIp}",
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                var result = await authService.LoginAsync(request);
                if (result is null)
                {
                    return BadRequest("Invalid username or password.");
                }
                else
                {
                    logger.LogInformation("POST Login successful.");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST Login failed.");
                throw;
            }
        }
        // PUT /auth/refresh-token
        [Authorize]
        [HttpPut("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "POST RefreshToken requested by user {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                if (clientId == null)
                {
                    return Unauthorized("Could not determine user from token.");
                }

                var (tokens, error) = await authService.RefreshTokensAsync(clientId.Value, request.RefreshToken);

                if (error != null)
                {
                    return Unauthorized(error);
                } else 
                {
                    logger.LogInformation("POST RefreshToken successful for user {ClientId}", clientId);
                    return Ok(tokens);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST RefreshToken failed for user {UserId}",
                    clientId);
                throw;
            }
        }

        // GET /auth/check-password
        [Authorize]
        [HttpGet("check-password")]
        public async Task<IActionResult> CheckPassword()
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "POST CheckPassword requested by user {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                if (clientId == null)
                {
                    return Unauthorized("Could not determine user from token.");
                }

                var status = await authService.CheckPasswordAsync(clientId);
                if (status is null)
                {
                    return NotFound();
                }
                else
                {
                    logger.LogInformation(
               "POST CheckPassword successful for user {UserId} from IP {RemoteIp}",
               clientId,
               HttpContext.Connection.RemoteIpAddress);
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST CheckPassword failed for user {UserId}",
                    clientId);
                throw;
            }
        }

        // PUT /auth/change-password
        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto body)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "POST ChangePassword requested by user {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                if (clientId == null)
                {
                    return Unauthorized("Could not determine user from token.");
                }

                var changed = await authService.UpdatePasswordAsync(clientId, body.CurrentPassword, body.NewPassword);
                if (!changed)
                {
                    return BadRequest("Current password is incorrect or user is not found.");
                }
                else
                {
                    logger.LogInformation("POST ChangePassword successful for user {ClientId}", clientId);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST ChangePassword failed for user {UserId}",
                    clientId);
                throw;
            }
        }

        // POST /auth/signout
        [Authorize]
        [HttpPost("signout")]
        public async Task<IActionResult> NewSignOut()
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "POST ChangePassword requested by user {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                if (clientId == null)
                {
                    return Unauthorized("Could not determine user from token.");
                }

                var result = await authService.SignOutAsync(clientId);
                if (!result)
                {
                    return BadRequest("Could not sign out user.");
                } else {
                    logger.LogInformation("POST SignOut successful for user {ClientId}", clientId);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST ChangePassword failed for user {UserId}",
                    clientId);
                throw;
            }
        }

        // GET /auth/profile
        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> CheckProfile()
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "POST Profile requested by user {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                if (clientId == null)
                {
                    return Unauthorized("Could not determine user from token.");
                }

                var result = await authService.GetProfileAsync(clientId);
                if (result is null)
                {
                    return NotFound();
                }
                else
                {
                    logger.LogInformation("POST Profile successful user {ClientId}", clientId);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST Profile failed for user {UserId}",
                    clientId);
                throw;
            }
        }
        // GET /auth/admin/profile/{userId}
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/profile/{userId:guid}")]
        public async Task<ActionResult<UserProfileDto>> CheckProfile(Guid userId)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "POST CheckProfile requested by admin {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                if (clientId == null)
                {
                    return Unauthorized("Could not determine user from token.");
                }

                var result = await authService.GetProfileAsync(userId);
                if (result is null)
                {
                    return NotFound();
                }
                else
                {
                    logger.LogInformation("POST CheckProfile successful for user {ClientId}", clientId);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST CheckProfile failed for admin {UserId}",
                    clientId);
                throw;
            }
        }

        // PUT /auth/admin/change-password
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/change-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] AdminPasswordChangeDto body)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "POST AdminChangePassword requested by admin {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                if (clientId == null)
                {
                    return Unauthorized("Could not determine user from token.");
                }

                var changed = await authService.AdminUpdatePasswordAsync(body.Id, body.NewPassword);
                if (!changed)
                {
                    return BadRequest("Current password is incorrect.");
                } else
                {
                    logger.LogInformation("POST AdminChangePassword successful for user {ClientId}", clientId);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST AdminChangePassword failed for admin {UserId}",
                    clientId);
                throw;
            }
        }

        // PUT /auth/admin/change-role
        [Authorize(Roles="Admin")]
        [HttpPut("admin/change-role")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleDto body)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "POST ChangeUserRole requested by admin {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                if (clientId == null)
                {
                    return Unauthorized("Could not determine user from token.");
                }

                var changed = await authService.UpdateUserRoleAsync(body.Id, body.NewRole);
                if (!changed)
                {
                    return BadRequest("Could not change user role.");
                }
                else 
                {
                    logger.LogInformation("POST ChangeUserRole successful for user {ClientId}", clientId);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST ChangeUserRole failed for admin {UserId}",
                    clientId);
                throw;
            }
        }
    }
}
