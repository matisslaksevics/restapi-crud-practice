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
    public class TokenService(IAuthRepository authRepository, IJwtSettingsService jwtSettings, BookBorrowingContext dbContext) : ITokenService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<TokenResponseDto> CreateTokenResponseAsync(Client user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(Guid userId, string refreshToken)
        {
            var user = await ValidateRefreshTokenAsync(userId, refreshToken);
            if (user is null)
            {
                return null;
            }

            return await CreateTokenResponseAsync(user);
        }

        private async Task<Client?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await _authRepository.GetClientByIdAsync(userId);
            if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow || string.IsNullOrEmpty(user.RefreshToken) || user.RefreshToken != refreshToken)
            {
                return null;
            }

            return user;
        }

        public string GenerateRefreshToken()
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

            await _authRepository.UpdateClientAsync(user);
            await dbContext.SaveChangesAsync();
            return refreshToken;
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