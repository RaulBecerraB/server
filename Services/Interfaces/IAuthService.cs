using server.Models.DTOs;

namespace server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<UserResponseDto?> GetUserByIdAsync(int userId);
    }
} 