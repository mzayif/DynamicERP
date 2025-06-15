using DynamicERP.Core.Entities;

namespace DynamicERP.Core.Interfaces.Repositories;

public interface ITenantRepository : IGenericRepository<Tenant, Guid>
{
    Task<Tenant?> GetByCodeAsync(string code);
    Task<bool> ExistsByCodeAsync(string code);
} 