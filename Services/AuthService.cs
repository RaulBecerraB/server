using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Models.DTOs;
using server.Services.Interfaces;
using server.Constants;

namespace server.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthService(
            ApplicationDbContext context,
            IPasswordService passwordService,
            IJwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto)
        {
            // Verificar si el usuario ya existe
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = AuthConstants.Messages.EmailAlreadyExists
                };
            }

            // Crear nuevo usuario
            var user = new User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PasswordHash = _passwordService.HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generar JWT token
            var token = _jwtService.GenerateToken(user);

            var userResponse = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt
            };

            return new AuthResultDto
            {
                Success = true,
                Message = AuthConstants.Messages.UserRegisteredSuccessfully,
                User = userResponse,
                Token = token
            };
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            // Buscar usuario por email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = AuthConstants.Messages.InvalidCredentials
                };
            }

            // Verificar contraseña
            if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = AuthConstants.Messages.InvalidCredentials
                };
            }

            // Generar JWT token
            var token = _jwtService.GenerateToken(user);

            var userResponse = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt
            };

            return new AuthResultDto
            {
                Success = true,
                Message = AuthConstants.Messages.LoginSuccessful,
                User = userResponse,
                Token = token
            };
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return null;

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt
            };
        }
    }
} 