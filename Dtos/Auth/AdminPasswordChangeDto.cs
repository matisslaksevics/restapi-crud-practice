namespace JwtAuthDotNet9.Dtos.Auth
{
    public class AdminPasswordChangeDto
    {
        public Guid Id { get; set; }
        public required string NewPassword { get; set; }
    }
}
