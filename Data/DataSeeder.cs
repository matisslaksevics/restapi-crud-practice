using restapi_crud_practice.Dtos.Auth;
using restapi_crud_practice.Services.SAuth;

namespace restapi_crud_practice.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAdminUserAsync(IAuthService authService, IConfiguration configuration)
        {
            try
            {
                var username = configuration["AdminCredentials:Username"];
                var password = configuration["AdminCredentials:Password"];

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Admin credentials not found in configuration.");
                    return;
                }

                var adminUserDto = new UserDto
                {
                    Username = username,
                    Password = password
                };

                var adminUser = await authService.RegisterAsync(adminUserDto);
                if (adminUser == null)
                {
                    Console.WriteLine("Admin user creation failed or already exists!");
                    return;
                }



                await authService.UpdateUserRoleAsync(adminUser.Id, "Admin");
                Console.WriteLine("Admin user created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding admin user: {ex.Message}");
            }
        }
    }
}