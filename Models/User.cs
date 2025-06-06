using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Relación con RefreshTokens
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
} 