using DynamicERP.Core.Entities;

namespace DynamicERP.Core.Interfaces.Repositories;

public interface IExternalProviderRepository : IGenericRepository<ExternalProvider, Guid>
{
    Task<ExternalProvider?> GetByCodeAsync(string code, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}