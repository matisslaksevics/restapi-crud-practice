using System.Security.Claims;

namespace restapi_crud_practice.Services.SUserContext
{
    public class UserContextService : IUserContextService
    {
        public Guid? GetUserId(ClaimsPrincipal user)
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