using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        
        [Required]
        public string Token { get; set; } = string.Empty;
        
        [Required]
        public int UserId { get; set; }
        
        public User User { get; set; } = null!;
        
        public DateTime ExpiresAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsRevoked { get; set; } = false;
        
        public DateTime? RevokedAt { get; set; }
        
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        
        public bool IsActive => !IsRevoked && !IsExpired;
    }
} 