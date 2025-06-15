using DynamicERP.Domain.Entities;

namespace DynamicERP.Domain.Interfaces;

public interface ITenantRepository : IGenericRepository<Tenant, Guid>
{
    Task<Tenant?> GetByCodeAsync(string code, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
} 