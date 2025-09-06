using restapi_crud_practice.Entities;
using restapi_crud_practice.Dtos.Auth;
using restapi_crud_practice.Dtos.Token;

namespace restapi_crud_practice.Services.SAuth
{
    public interface IAuthService
    {
        Task<Client?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(Guid userId, string refreshToken);
        Task<CheckPasswordDto?> CheckPasswordAsync(Guid? userId);
        Task<bool> ChangePasswordAsync(Guid? userId, string currentPassword, string newPassword);
        Task<bool> SignOutAsync(Guid? userId);
        Task<UserProfileDto?> GetProfileAsync(Guid? userId);
        Task<bool> AdminSetPasswordAsync(Guid? userId, string newPassword);
        Task<bool> ChangeUserRoleAsync(Guid? userId, string newRole);
    }
}
