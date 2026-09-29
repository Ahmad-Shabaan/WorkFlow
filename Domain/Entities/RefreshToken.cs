using Domain.Common;

namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = default!;
        public int UserId { get; set; } = default!;
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }
    }
}
