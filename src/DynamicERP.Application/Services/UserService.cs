using DynamicERP.Core.Entities;
using DynamicERP.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace DynamicERP.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _userRepository.GetByUsernameAsync(username);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User> CreateUserAsync(User user, string? password = null)
    {
        if (await _userRepository.ExistsAsync(user.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        if (await _userRepository.ExistsByUsernameAsync(user.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        if (!string.IsNullOrEmpty(password))
        {
            user.PasswordHash = HashPassword(password);
        }

        user.CreatedAt = DateTime.UtcNow;
        user.IsActive = true;

        return await _userRepository.AddAsync(user);
    }

    public async Task<User> CreateExternalUserAsync(User user, string externalId, string provider)
    {
        if (await _userRepository.ExistsAsync(user.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        if (await _userRepository.ExistsByUsernameAsync(user.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        user.ExternalId = externalId;
        user.ExternalProvider = provider;
        user.CreatedAt = DateTime.UtcNow;
        user.IsActive = true;

        return await _userRepository.AddAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        var existingUser = await _userRepository.GetByIdAsync(user.Id);
        if (existingUser == null)
        {
            throw new InvalidOperationException("User not found");
        }

        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || string.IsNullOrEmpty(user.PasswordHash))
        {
            return false;
        }

        return VerifyPassword(password, user.PasswordHash);
    }

    public async Task<bool> ValidateExternalCredentialsAsync(string externalId, string provider)
    {
        var user = await _userRepository.GetByExternalIdAsync(externalId, provider);
        return user != null && user.IsActive;
    }

    public async Task UpdateLastLoginAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
} 