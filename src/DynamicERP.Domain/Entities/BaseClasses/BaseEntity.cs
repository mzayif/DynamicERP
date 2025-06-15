namespace DynamicERP.Domain.Entities.BaseClasses;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;
} 