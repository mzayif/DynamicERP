using DynamicERP.Core.Entities;

namespace DynamicERP.Core.Interfaces.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> CreateUserAsync(User user, string? password = null);
    Task<User> CreateExternalUserAsync(User user, string externalId, string provider);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid id);
    Task<bool> ValidateCredentialsAsync(string email, string password);
    Task<bool> ValidateExternalCredentialsAsync(string externalId, string provider);
    Task UpdateLastLoginAsync(Guid userId);
} 