using System.ComponentModel.DataAnnotations;

namespace server.Models.DTOs
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
} 