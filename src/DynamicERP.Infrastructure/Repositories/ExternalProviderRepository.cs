using DynamicERP.Core.Entities;
using DynamicERP.Core.Interfaces;
using DynamicERP.Core.Interfaces.Repositories;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure.Repositories;

public class ExternalProviderRepository : GenericRepository<ExternalProvider, Guid>, IExternalProviderRepository
{
    public ExternalProviderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ExternalProvider?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Code == code);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _dbSet.AnyAsync(p => p.Code == code);
    }
} 