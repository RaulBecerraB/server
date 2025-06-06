namespace server.Models.DTOs
{
    public class AuthResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserResponseDto? User { get; set; }
        public TokenResponseDto? Tokens { get; set; }
    }
} 