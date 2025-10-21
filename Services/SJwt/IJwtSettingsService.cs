namespace restapi_crud_practice.Services.SJwt
{
    public interface IJwtSettingsService
    {
        string GetTokenSecret();
        string GetIssuer();
        string GetAudience();
    }
}
