using DynamicERP.Domain.Entities.BaseClasses;

namespace DynamicERP.Domain.Entities;

public class RefreshToken : BaseFullEntity
{
    public RefreshToken()
    {
        IsActive = RevokedAt == null && !IsExpired;
    }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedBy { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    // Foreign key
    public Guid UserId { get; set; }

    // Navigation property
    public User User { get; set; } = null!;
}