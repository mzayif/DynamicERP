using DynamicERP.Core.Entities;
using DynamicERP.Core.Interfaces.Repositories;
using DynamicERP.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;

    public TenantService(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _tenantRepository.GetByIdAsync(id, false, cancellationToken);
    }

    public async Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tenants = await _tenantRepository.GetAll().ToListAsync(cancellationToken);
        return tenants.ToList();
    }

    public async Task<Tenant> AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        await _tenantRepository.AddAsync(tenant, cancellationToken);
        return tenant;
    }

    public async Task<Tenant> UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        await _tenantRepository.UpdateAsync(tenant, cancellationToken);
        return tenant;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _tenantRepository.DeleteAsync(id, cancellationToken);
    }
} 