using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using server.Data;
using server.Models;
using server.Models.DTOs;
using server.Services.Interfaces;
using server.Constants;

namespace server.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public RefreshTokenService(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateSecureToken(),
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(30) // 30 días de duración
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null || !storedToken.IsActive)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Refresh token inválido o expirado"
                };
            }

            // Revocar el refresh token usado (rotación de tokens)
            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;

            // Generar nuevos tokens
            var newAccessToken = _jwtService.GenerateToken(storedToken.User);
            var newRefreshToken = await GenerateRefreshTokenAsync(storedToken.UserId);

            await _context.SaveChangesAsync();

            var userResponse = new UserResponseDto
            {
                Id = storedToken.User.Id,
                Email = storedToken.User.Email,
                FirstName = storedToken.User.FirstName,
                LastName = storedToken.User.LastName,
                CreatedAt = storedToken.User.CreatedAt
            };

            return new AuthResultDto
            {
                Success = true,
                Message = "Tokens renovados exitosamente",
                User = userResponse,
                Tokens = new TokenResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken.Token,
                    AccessTokenExpires = DateTime.UtcNow.AddMinutes(15), // 15 minutos
                    RefreshTokenExpires = newRefreshToken.ExpiresAt
                }
            };
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null || storedToken.IsRevoked)
                return false;

            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RevokeAllUserRefreshTokensAsync(int userId)
        {
            var userTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.IsActive)
                .ToListAsync();

            foreach (var token in userTokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
        }

        private static string GenerateSecureToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
} 