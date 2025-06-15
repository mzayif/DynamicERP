using DynamicERP.Core.Entities;

namespace DynamicERP.Core.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByExternalIdAsync(string externalId, string provider);
    Task<bool> ExistsAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);
} 