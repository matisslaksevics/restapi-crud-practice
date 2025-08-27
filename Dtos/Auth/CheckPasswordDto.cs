namespace JwtAuthDotNet9.Dtos.Auth
{
    public class CheckPasswordDto
    {
        public bool Valid { get; set; }
        public DateTime? PasswordChangedAt { get; set; }
        public int PasswordMaxAgeDays { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsExpired { get; set; }
        public int? DaysRemaining { get; set; }
    }
}