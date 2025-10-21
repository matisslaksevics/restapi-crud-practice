using System.Security.Claims;
namespace restapi_crud_practice.Services.SUserContext
{
    public interface IUserContextService
    {
        Guid? GetUserId(ClaimsPrincipal user);
    }
}
