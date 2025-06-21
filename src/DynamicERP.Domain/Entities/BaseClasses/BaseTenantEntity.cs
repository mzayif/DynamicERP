using DynamicERP.Core.Constants;

namespace DynamicERP.Domain.Entities.BaseClasses;

public abstract class BaseTenantEntity : BaseFullEntity
{
    public Guid TenantId { get; set; } = DefaultCodes.TestTenant; // Varsay�lan tenant ID'si, test i�in kullan�labilir
} 