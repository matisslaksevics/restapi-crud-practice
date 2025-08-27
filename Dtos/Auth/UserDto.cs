namespace JwtAuthDotNet9.Dtos.Auth
{
    public class UserDto
    {
        public required string Username { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
