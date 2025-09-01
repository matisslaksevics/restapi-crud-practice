using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace restapi_crud_practice.Services.SAuth
{
    public class AuthService(BookBorrowingContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            var user = await context.Clients.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<Client>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDto> CreateTokenResponse(Client user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        public async Task<Client?> RegisterAsync(UserDto request)
        {
            if (await context.Clients.AnyAsync(u => u.Username == request.Username))
            {
                return null;
            }

            var user = new Client();
            var hashedPassword = new PasswordHasher<Client>()
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            user.Role = "User";

            context.Clients.Add(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(Guid? userId, string refreshToken)
        {
            var user = await ValidateRefreshTokenAsync(userId, refreshToken);
            if (user is null)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        private async Task<Client?> ValidateRefreshTokenAsync(Guid? userId, string refreshToken)
        {
            var user = await context.Clients.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(Client user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        private string CreateToken(Client user)
        {
            var claims = new List<Claim>
            {
               new (ClaimTypes.NameIdentifier, user.Id.ToString()),
               new (ClaimTypes.Name, user.Username),
               new (ClaimTypes.Role, user.Role ?? "User")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public async Task<CheckPasswordDto?> CheckPasswordAsync(Guid? userId)
        {
            var user = await context.Clients.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
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
                return (null, false, null); // expiresAt = null, isExpired = false, daysRemaining = null
            }

            var expiresAt = user.PasswordChangedAt.AddDays(user.PasswordMaxAgeDays);
            var now = DateTime.UtcNow;
            var isExpired = now >= expiresAt;
            var daysRemaining = isExpired ? 0 : (int)Math.Ceiling((expiresAt - now).TotalDays);
            return (expiresAt, isExpired, daysRemaining);
        }

        public async Task<bool> ChangePasswordAsync(Guid? userId, string currentPassword, string newPassword)
        {
            var user = await context.Clients.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                return false;
            } 

            var hasher = new PasswordHasher<Client>();
            var verify = hasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword);
            if (verify == PasswordVerificationResult.Failed)
            {
                return false;
            } 

            user.PasswordHash = hasher.HashPassword(user, newPassword);

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task SignOutAsync(Guid? userId)
        {
            var user = await context.Clients.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                return;
            } 
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await context.SaveChangesAsync();
        }
        public async Task<UserProfileDto?> GetProfileAsync(Guid? userId)
        {
            var user = await context.Clients.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
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
        public async Task<bool> AdminSetPasswordAsync(Guid? userId, string newPassword)
        {
            var user = await context.Clients.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                return false;
            }
           
            var hasher = new PasswordHasher<Client>();
            user.PasswordHash = hasher.HashPassword(user, newPassword);
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ChangeUserRoleAsync(Guid? userId, string newRole)
        {
            var user = await context.Clients.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                return false;
            }

            user.Role = newRole;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
