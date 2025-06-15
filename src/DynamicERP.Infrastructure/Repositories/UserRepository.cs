using DynamicERP.Domain.Entities;
using DynamicERP.Domain.Interfaces;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(u => u.Email == email, isTracking, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByExternalIdAsync(string externalId, string provider)
    {
        return await DbSet.FirstOrDefaultAsync(u =>
            u.ExternalId == externalId && u.ExternalProvider == provider);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Find(u => u.Email == email).AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await DbSet.AnyAsync(u => u.Username == username);
    }
}