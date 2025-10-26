using Microsoft.AspNetCore.Identity;
using restapi_crud_practice.Dtos.Auth;
using restapi_crud_practice.Dtos.Token;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Repositories;
using restapi_crud_practice.Services.SToken;

namespace restapi_crud_practice.Services.SAuth
{
    public class AuthService(IAuthRepository authRepository, ITokenService tokenService) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly ITokenService _tokenService = tokenService;

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var dummyUser = new Client();
            return new PasswordHasher<Client>()
                .VerifyHashedPassword(dummyUser, hashedPassword, providedPassword)
                != PasswordVerificationResult.Failed;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            var user = await _authRepository.GetClientByUsernameAsync(request.Username);
            if (user is null)
            {
                return null;
            }

            var hashedPassword = await _authRepository.GetHashedPasswordAsync(user.Id);
            if (hashedPassword == null) return null;

            if (!VerifyPassword(hashedPassword, request.Password))
                return null;

            return await _tokenService.CreateTokenResponseAsync(user);
        }

        public async Task<Client?> RegisterAsync(UserDto request)
        {
            var userExists = await _authRepository.GetClientByUsernameAsync(request.Username);
            if (userExists is not null)
            {
                return null;
            }

            var user = new Client();
            var hashedPassword = new PasswordHasher<Client>()
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            user.Role = "User";

            await _authRepository.CreateClientAsync(user);

            return user;
        }

        public async Task<(TokenResponseDto? Tokens, string? Error)> RefreshTokensAsync(Guid userId, string refreshToken)
        {
            var tokenResult = string.IsNullOrWhiteSpace(refreshToken);
            if (tokenResult)
            {
                return (null, "Refresh token is required");
            }

            var user = await _authRepository.GetClientByIdAsync(userId);
            if (user is null)
            {
                return (null, "User not found");
            }

            if (user.RefreshToken != refreshToken)
            {
                return (null, "Invalid refresh token");
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return (null, "Refresh token has expired");
            }

            var result = await _tokenService.RefreshTokensAsync(userId, refreshToken);
            if (result is null)
            {
                return (null, "Failed to generate new tokens");
            }

            return (result, null);
        }

        public async Task<CheckPasswordDto?> CheckPasswordAsync(Guid? userId)
        {
            if (userId is null)
            {
                return null;
            }
            var user = await _authRepository.GetClientByIdAsync(userId.Value);
            if (user is null)
            {
                return null;
            }

            var (expiresAt, isExpired, daysRemaining) = ComputePasswordStatus(user);
            return new CheckPasswordDto
            {
                Valid = !isExpired,
                PasswordChangedAt = user.PasswordChangedAt,
                PasswordMaxAgeDays = user.PasswordMaxAgeDays,
                ExpiresAt = expiresAt,
                IsExpired = isExpired,
                DaysRemaining = daysRemaining
            };
        }

        private static (DateTime? expiresAt, bool isExpired, int? daysRemaining) ComputePasswordStatus(Client user)
        {
            if (user.PasswordMaxAgeDays <= 0)
            {
                return (null, false, null);
            }

            var expiresAt = user.PasswordChangedAt.AddDays(user.PasswordMaxAgeDays);
            var now = DateTime.UtcNow;
            var isExpired = now >= expiresAt;
            var daysRemaining = isExpired ? 0 : (int)Math.Ceiling((expiresAt - now).TotalDays);
            return (expiresAt, isExpired, daysRemaining);
        }

        public async Task<bool> UpdatePasswordAsync(Guid? userId, string currentPassword, string newPassword)
        {
            if (userId is null)
            {
                return false;
            }
            var user = await _authRepository.GetClientByIdAsync(userId.Value);
            if (user is null)
            {
                return false;
            }

            var hashedPassword = await _authRepository.GetHashedPasswordAsync(user.Id);
            if (hashedPassword == null || !VerifyPassword(hashedPassword, currentPassword))
            {
                return false;
            }

            var hasher = new PasswordHasher<Client>();
            user.PasswordHash = hasher.HashPassword(user, newPassword);
            RefreshTokenHelper.InvalidateRefreshToken(user);

            await _authRepository.UpdateClientAsync(user);
            return true;
        }

        public async Task<bool> SignOutAsync(Guid? userId)
        {
            if (userId is null)
            {
                return false;
            }
            var user = await _authRepository.GetClientByIdAsync(userId.Value);
            if (user is null)
            {
                return false;
            }

            RefreshTokenHelper.InvalidateRefreshToken(user);
            await _authRepository.UpdateClientAsync(user);

            return true;
        }

        public async Task<UserProfileDto?> GetProfileAsync(Guid? userId)
        {
            if (userId is null)
            {
                return null;
            }
            var user = await _authRepository.GetClientByIdAsync(userId.Value);
            if (user is null)
            {
                return null;
            }

            return new UserProfileDto
            {
                Username = user.Username,
                Role = user.Role ?? "User"
            };
        }

        public async Task<bool> AdminUpdatePasswordAsync(Guid? userId, string newPassword)
        {
            if (userId is null)
            {
                return false;
            }
            var user = await _authRepository.GetClientByIdAsync(userId.Value);
            if (user is null)
            {
                return false;
            }

            var hasher = new PasswordHasher<Client>();
            user.PasswordHash = hasher.HashPassword(user, newPassword);
            RefreshTokenHelper.InvalidateRefreshToken(user);

            await _authRepository.UpdateClientAsync(user);
            return true;
        }

        public async Task<bool> UpdateUserRoleAsync(Guid? userId, string newRole)
        {
            if (userId is null)
            {
                return false;
            }
            var user = await _authRepository.GetClientByIdAsync(userId.Value);
            if (user is null)
            {
                return false;
            }

            user.Role = newRole;
            await _authRepository.UpdateClientAsync(user);
            return true;
        }
    }
}