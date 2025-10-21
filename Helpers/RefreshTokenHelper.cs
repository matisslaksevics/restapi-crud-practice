using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Helpers
{
    public static class RefreshTokenHelper
    {
        public static void InvalidateRefreshToken(Client user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
         }
    }
}
