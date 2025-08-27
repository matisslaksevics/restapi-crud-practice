namespace JwtAuthDotNet9.Dtos.Auth
{
    public class ChangeUserRoleDto
    {
        public required Guid Id { get; set; }
        public required string NewRole { get; set; }
    }
}
