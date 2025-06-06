using server.Constants;
using server.Models.DTOs;

namespace server.Helpers
{
    public static class CookieHelper
    {
        public static CookieOptions GetAccessTokenCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Solo HTTPS en producción
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(15) // Corta duración
            };
        }

        public static CookieOptions GetRefreshTokenCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Solo HTTPS en producción
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30) // Larga duración
            };
        }

        public static void SetAuthCookies(HttpResponse response, TokenResponseDto tokens)
        {
            response.Cookies.Append(AuthConstants.JwtCookieName, tokens.AccessToken, GetAccessTokenCookieOptions());
            response.Cookies.Append(AuthConstants.RefreshTokenCookieName, tokens.RefreshToken, GetRefreshTokenCookieOptions());
        }

        public static void DeleteAuthCookies(HttpResponse response)
        {
            response.Cookies.Delete(AuthConstants.JwtCookieName);
            response.Cookies.Delete(AuthConstants.RefreshTokenCookieName);
        }

        // Métodos legacy para compatibilidad
        public static CookieOptions GetJwtCookieOptions() => GetAccessTokenCookieOptions();
        
        public static void SetJwtCookie(HttpResponse response, string token)
        {
            response.Cookies.Append(AuthConstants.JwtCookieName, token, GetAccessTokenCookieOptions());
        }

        public static void DeleteJwtCookie(HttpResponse response)
        {
            response.Cookies.Delete(AuthConstants.JwtCookieName);
        }
    }
} 