using System.Security.Claims;
using server.Models;

namespace server.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
    }
} 