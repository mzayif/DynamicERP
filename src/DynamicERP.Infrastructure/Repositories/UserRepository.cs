using DynamicERP.Core.Entities;
using DynamicERP.Core.Interfaces;
using DynamicERP.Core.Interfaces.Repositories;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByExternalIdAsync(string externalId, string provider)
    {
        return await _dbSet.FirstOrDefaultAsync(u => 
            u.ExternalId == externalId && u.ExternalProvider == provider);
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _dbSet.AnyAsync(u => u.Username == username);
    }
} 