using DynamicERP.Domain.Entities;
using DynamicERP.Domain.Interfaces;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure.Repositories;

public class ExternalProviderRepository : GenericRepository<ExternalProvider, Guid>, IExternalProviderRepository
{
    public ExternalProviderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ExternalProvider?> GetByCodeAsync(string code, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(p => p.Code == code, isTracking, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var query = Find(p => p.Code == code, false);
        return await query.AnyAsync(cancellationToken);
    }
} 