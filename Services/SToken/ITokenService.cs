using restapi_crud_practice.Dtos.Token;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Services.SToken
{
    public interface ITokenService
    {
        Task<TokenResponseDto> CreateTokenResponseAsync(Client user);
        Task<TokenResponseDto?> RefreshTokensAsync(Guid userId, string refreshToken);
        string GenerateRefreshToken();
        string CreateToken(Client user);
    }
}
