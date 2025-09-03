using restapi_crud_practice.Dtos.Auth;
using restapi_crud_practice.Services.SAuth;
namespace restapi_crud_practice.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAdminUserAsync(IAuthService authService)
        {
            try
            {
                var adminUserDto = new UserDto
                {
                    Username = "admin",
                    Password = "admin123"
                };

                var adminUser = await authService.RegisterAsync(adminUserDto);
                if (adminUser != null)
                {
                    await authService.ChangeUserRoleAsync(adminUser.Id, "Admin");
                    Console.WriteLine("Admin user created successfully!");
                }
                else
                {
                    Console.WriteLine("Admin user already exists or creation failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding admin user: {ex.Message}");
            }
        }
    }
}

