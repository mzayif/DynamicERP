using DynamicERP.Domain.Entities;
using DynamicERP.Domain.Interfaces;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure.Repositories;

public class TenantRepository : GenericRepository<Tenant, Guid>, ITenantRepository
{
    public TenantRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Tenant?> GetByCodeAsync(string code, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(t => t.Code == code, isTracking, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await Find(t => t.Code == code, false).AnyAsync(cancellationToken);
    }
}