using DynamicERP.Domain.Entities;

namespace DynamicERP.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByExternalIdAsync(string externalId, string provider);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username);
} 