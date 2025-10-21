namespace restapi_crud_practice.Dtos.Auth
{
    public abstract class ChangePasswordBaseDto
    {
        public required string NewPassword { get; set; }
    }
}