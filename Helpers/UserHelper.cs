using System.Security.Claims;

namespace restapi_crud_practice.Helpers
{
    public static class UserHelper
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
    }
}