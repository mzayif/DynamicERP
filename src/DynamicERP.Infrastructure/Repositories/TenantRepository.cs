using DynamicERP.Core.Entities;
using DynamicERP.Core.Interfaces;
using DynamicERP.Core.Interfaces.Repositories;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure.Repositories;

public class TenantRepository : GenericRepository<Tenant, Guid>, ITenantRepository
{
    public TenantRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Tenant?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Code == code);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _dbSet.AnyAsync(t => t.Code == code);
    }
} 