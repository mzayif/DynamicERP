using DynamicERP.Core.Entities;

namespace DynamicERP.Core.Interfaces.Repositories;

public interface IExternalProviderRepository : IGenericRepository<ExternalProvider, Guid>
{
    Task<ExternalProvider?> GetByCodeAsync(string code);
    Task<bool> ExistsByCodeAsync(string code);
} 