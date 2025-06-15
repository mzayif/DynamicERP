namespace DynamicERP.Core.Entities.BaseClasses;

public abstract class BaseCreateEntity<TKey> : BaseEntity<TKey>
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}