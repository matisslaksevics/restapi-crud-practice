namespace restapi_crud_practice.Dtos.Auth
{
    public class AdminPasswordChangeDto
    {
        public Guid Id { get; set; }
        public required string NewPassword { get; set; }
    }
}
