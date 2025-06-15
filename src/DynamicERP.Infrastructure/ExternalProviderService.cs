using DynamicERP.Core.Entities;
using DynamicERP.Core.Interfaces.Repositories;
using DynamicERP.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure;

public class ExternalProviderService : IExternalProviderService
{
    private readonly IExternalProviderRepository _externalProviderRepository;

    public ExternalProviderService(IExternalProviderRepository externalProviderRepository)
    {
        _externalProviderRepository = externalProviderRepository;
    }

    public async Task<ExternalProvider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _externalProviderRepository.GetByIdAsync(id, false, cancellationToken);
    }

    public async Task<IEnumerable<ExternalProvider>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var providers = await _externalProviderRepository.GetAll().ToListAsync(cancellationToken);
        return providers.ToList();
    }

    public async Task<ExternalProvider> AddAsync(ExternalProvider provider, CancellationToken cancellationToken = default)
    {
        await _externalProviderRepository.AddAsync(provider, cancellationToken);
        return provider;
    }

    public async Task<ExternalProvider> UpdateAsync(ExternalProvider provider, CancellationToken cancellationToken = default)
    {
        await _externalProviderRepository.UpdateAsync(provider, cancellationToken);
        return provider;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _externalProviderRepository.DeleteAsync(id, cancellationToken);
    }
} 