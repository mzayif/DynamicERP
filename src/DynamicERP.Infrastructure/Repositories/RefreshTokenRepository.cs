using DynamicERP.Domain.Entities;
using DynamicERP.Domain.Interfaces;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken, Guid>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();
        if (!isTracking)
            query = query.AsNoTracking();
            
        return await query.FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking()
            .Where(rt => rt.UserId == userId && rt.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task RevokeTokenAsync(string token, string reason, string revokedBy, CancellationToken cancellationToken = default)
    {
        var refreshToken = await GetByTokenAsync(token, true, cancellationToken);
        if (refreshToken != null)
        {
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedBy = revokedBy;
            refreshToken.ReasonRevoked = reason;
            await UpdateAsync(refreshToken, cancellationToken);
        }
    }

    public async Task RevokeAllUserTokensAsync(Guid userId, string reason, string revokedBy, CancellationToken cancellationToken = default)
    {
        var activeTokens = await GetActiveTokensByUserIdAsync(userId, cancellationToken);
        foreach (var token in activeTokens)
        {
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedBy = revokedBy;
            token.ReasonRevoked = reason;
            await UpdateAsync(token, cancellationToken);
        }
    }

    public async Task<bool> IsTokenActiveAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = await GetByTokenAsync(token, false, cancellationToken);
        return refreshToken?.IsActive ?? false;
    }
} 