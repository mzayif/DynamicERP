using DynamicERP.Core.Entities;

namespace DynamicERP.Core.Interfaces.Services;

public interface IExternalProviderService
{
    Task<ExternalProvider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExternalProvider>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ExternalProvider> AddAsync(ExternalProvider provider, CancellationToken cancellationToken = default);
    Task<ExternalProvider> UpdateAsync(ExternalProvider provider, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
} 