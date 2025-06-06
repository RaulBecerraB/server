namespace server.Constants
{
    public static class AuthConstants
    {
        public const string JwtCookieName = "jwt";
        public const string CorsPolicy = "AllowFrontend";
        
        public static class Messages
        {
            public const string EmailAlreadyExists = "El email ya está registrado";
            public const string InvalidCredentials = "Credenciales inválidas";
            public const string UserRegisteredSuccessfully = "Usuario registrado exitosamente";
            public const string LoginSuccessful = "Login exitoso";
            public const string LogoutSuccessful = "Logout exitoso";
        }

        public static class ConfigSections
        {
            public const string JwtSettings = "JwtSettings";
            public const string ConnectionStrings = "ConnectionStrings";
        }

        public static class JwtSettings
        {
            public const string SecretKey = "SecretKey";
            public const string Issuer = "Issuer";
            public const string Audience = "Audience";
            public const string ExpirationInDays = "ExpirationInDays";
        }
    }
} 