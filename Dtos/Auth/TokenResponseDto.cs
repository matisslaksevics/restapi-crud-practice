namespace JwtAuthDotNet9.Dtos.Auth
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
