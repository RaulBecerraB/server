using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using server.Models;
using server.Services.Interfaces;
using server.Constants;

namespace server.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection(AuthConstants.ConfigSections.JwtSettings);
            var key = Encoding.ASCII.GetBytes(jwtSettings[AuthConstants.JwtSettings.SecretKey]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationInMinutes"])),
                Issuer = jwtSettings[AuthConstants.JwtSettings.Issuer],
                Audience = jwtSettings[AuthConstants.JwtSettings.Audience],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection(AuthConstants.ConfigSections.JwtSettings);
                var key = Encoding.ASCII.GetBytes(jwtSettings[AuthConstants.JwtSettings.SecretKey]!);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings[AuthConstants.JwtSettings.Issuer],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings[AuthConstants.JwtSettings.Audience],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
} 