using DynamicERP.Core.Entities;
using DynamicERP.Core.Interfaces.Repositories;
using DynamicERP.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetByIdAsync(id, false, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _userRepository.Find(u=>u.Email.ToLower().Equals(email.ToLower())).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _userRepository.Find(u => u.Username.ToLower().Equals(username.ToLower())).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAll(false).ToListAsync(cancellationToken);
        return users.ToList();
    }

    public Task<User> CreateUserAsync(User user, string? password = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<User> CreateExternalUserAsync(User user, string externalId, string provider, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateExternalCredentialsAsync(string externalId, string provider,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateLastLoginAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
} 