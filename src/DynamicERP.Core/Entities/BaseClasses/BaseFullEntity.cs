namespace DynamicERP.Core.Entities.BaseClasses;

public abstract class BaseFullEntity : BaseCreateEntity<Guid>
{
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsActive { get; set; } = true;
}