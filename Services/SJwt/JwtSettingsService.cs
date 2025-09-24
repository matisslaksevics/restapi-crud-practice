using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Services.SJwt
{
    public class JwtSettingsService : IJwtSettingsService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtSettingsService(IConfiguration configuration)
        {
            _jwtSettings = configuration.GetSection("AppSettings").Get<JwtSettings>()
                          ?? throw new InvalidOperationException("JWT settings are missing from configuration.");
            if (string.IsNullOrWhiteSpace(_jwtSettings.Token))
            {
                throw new InvalidOperationException("JWT Token secret is required in appsettings.json");
            }
        }

        public string GetTokenSecret() => _jwtSettings.Token;
        public string GetIssuer() => _jwtSettings.Issuer;
        public string GetAudience() => _jwtSettings.Audience;
    }
}