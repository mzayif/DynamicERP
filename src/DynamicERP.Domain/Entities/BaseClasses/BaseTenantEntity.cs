namespace DynamicERP.Domain.Entities.BaseClasses;

public abstract class BaseTenantEntity : BaseFullEntity
{
    public Guid TenantId { get; set; }
} 