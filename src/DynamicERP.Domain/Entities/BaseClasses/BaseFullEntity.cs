namespace DynamicERP.Domain.Entities.BaseClasses;

public abstract class BaseFullEntity : BaseCreateEntity<Guid>
{
    protected BaseFullEntity()
    {
        Id = Guid.CreateVersion7();
    }

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsActive { get; set; } = true;
} 