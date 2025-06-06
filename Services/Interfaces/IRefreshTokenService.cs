using server.Models;
using server.Models.DTOs;

namespace server.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GenerateRefreshTokenAsync(int userId);
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
        Task RevokeAllUserRefreshTokensAsync(int userId);
        Task CleanupExpiredTokensAsync();
    }
} 