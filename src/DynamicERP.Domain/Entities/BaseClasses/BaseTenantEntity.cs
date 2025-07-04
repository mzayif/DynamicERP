using DynamicERP.Core.Constants;

namespace DynamicERP.Domain.Entities.BaseClasses;

public abstract class BaseTenantEntity : BaseFullEntity
{
    public Guid TenantId { get; set; } = DefaultCodes.TestTenant; // Varsayılan tenant ID'si, test için kullanılabilir
} 