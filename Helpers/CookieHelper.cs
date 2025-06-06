using server.Constants;

namespace server.Helpers
{
    public static class CookieHelper
    {
        public static CookieOptions GetJwtCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Solo HTTPS en producción
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
        }

        public static void SetJwtCookie(HttpResponse response, string token)
        {
            response.Cookies.Append(AuthConstants.JwtCookieName, token, GetJwtCookieOptions());
        }

        public static void DeleteJwtCookie(HttpResponse response)
        {
            response.Cookies.Delete(AuthConstants.JwtCookieName);
        }
    }
} 