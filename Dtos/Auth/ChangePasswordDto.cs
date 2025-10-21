namespace restapi_crud_practice.Dtos.Auth
{
    public class ChangePasswordDto : ChangePasswordBaseDto
    {
        public required string CurrentPassword { get; set; }
    }
}
