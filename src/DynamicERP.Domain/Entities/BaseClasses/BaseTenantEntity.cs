using DynamicERP.Core.Constants;

namespace DynamicERP.Domain.Entities.BaseClasses;

public abstract class BaseTenantEntity : BaseFullEntity
{
    public Guid TenantId { get; set; } = DefaultCodes.TestTenant; // Varsayýlan tenant ID'si, test için kullanýlabilir
} 