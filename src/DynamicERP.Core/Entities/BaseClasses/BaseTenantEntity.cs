namespace DynamicERP.Core.Entities.BaseClasses;

public abstract class BaseTenantEntity : BaseFullEntity
{
    public Guid TenantId { get; set; }
}