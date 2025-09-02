using Microsoft.AspNetCore.Identity;
using restapi_crud_practice.Entities;
using System.Security.Claims;

namespace restapi_crud_practice.Helpers
{
    public static class ClientHelper
    {
        public static Guid? GetUserId(ClaimsPrincipal user)
        {
            var idStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(idStr, out var userId)) 
            {
                return userId; 
            }

            return null; 
        }
        public static bool VerifyPassword<TUser>(TUser user, string hashedPassword, string providedPassword) where TUser : class
        {
            return new PasswordHasher<TUser>().VerifyHashedPassword(user, hashedPassword, providedPassword) != PasswordVerificationResult.Failed;

        }
        public static class RefreshTokenHelper
        {
            public static void InvalidateRefreshToken(Client user)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
            }
        }
    }
}