using Microsoft.IdentityModel.Tokens;
using restapi_crud_practice.Data;
using restapi_crud_practice.Dtos.Token;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Repositories;
using restapi_crud_practice.Services.SJwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace restapi_crud_practice.Services.SToken
{
    public class TokenService(IAuthRepository authRepository, IJwtSettingsService jwtSettings, BookBorrowingContext dbContext, ILogger<TokenService> logger) : ITokenService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<TokenResponseDto> CreateTokenResponseAsync(Client user)
        {
            logger.LogInformation("Service: CreateTokenResponseAsync requested.");
            try
            {
                var result =  new TokenResponseDto
                {
                    AccessToken = CreateToken(user),
                    RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
                };

                logger.LogInformation("Service: CreateTokenResponseAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: CreateTokenResponseAsync failed.");
                throw;
            }
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(Guid userId, string refreshToken)
        {
            logger.LogInformation("Service: RefreshTokensAsync requested.");
            try
            {
                var user = await ValidateRefreshTokenAsync(userId, refreshToken);
                if (user is null)
                {
                    logger.LogError("Service: RefreshTokensAsync failed.");
                    return null;
                } else
                {
                    logger.LogInformation("Service: RefreshTokensAsync successful.");
                    return await CreateTokenResponseAsync(user);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: RefreshTokensAsync failed.");
                throw;
            }
        }

        private async Task<Client?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            logger.LogInformation("Service: ValidateRefreshTokenAsync requested.");
            try
            {
                var user = await _authRepository.GetClientByIdAsync(userId);
                if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow || string.IsNullOrEmpty(user.RefreshToken) || user.RefreshToken != refreshToken)
                {
                    logger.LogError("Service: ValidateRefreshTokenAsync failed.");
                    return null;
                } else
                {
                    logger.LogInformation("Service: ValidateRefreshTokenAsync successful.");
                    return user;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: ValidateRefreshTokenAsync failed.");
                throw;
            }
        }

        public string GenerateRefreshToken()
        {
            logger.LogInformation("Service: GenerateRefreshToken requested.");
            try
            {
                var randomNumber = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                logger.LogInformation("Service: GenerateRefreshToken successful.");
                return Convert.ToBase64String(randomNumber);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: GenerateRefreshToken failed.");
                throw;
            }
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(Client user)
        {
            logger.LogInformation("Service: GenerateAndSaveRefreshTokenAsync requested.");
            try
            {
                var refreshToken = GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                var updated = await _authRepository.UpdateClientAsync(user);
                if(!updated)
                {
                    logger.LogError("Service: GenerateAndSaveRefreshTokenAsync failed.");
                }
                var saved = await dbContext.SaveChangesAsync();
                if (saved == 0)
                {
                    logger.LogError("Service: GenerateAndSaveRefreshTokenAsync failed.");
                }
                logger.LogInformation("Service: GenerateAndSaveRefreshTokenAsync successful.");
                return refreshToken;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: GenerateAndSaveRefreshTokenAsync failed.");
                throw;
            }
        }

        public string CreateToken(Client user)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Role, user.Role ?? "User")
            };

            var tokenSecret = jwtSettings.GetTokenSecret();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: jwtSettings.GetIssuer(),
                audience: jwtSettings.GetAudience(),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}