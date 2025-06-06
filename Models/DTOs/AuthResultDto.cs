namespace server.Models.DTOs
{
    public class AuthResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserResponseDto? User { get; set; }
        public string? Token { get; set; }
    }
} 