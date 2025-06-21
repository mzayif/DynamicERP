using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Domain.Entities;
using DynamicERP.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;
using DynamicERP.Core.Constants;

namespace DynamicERP.Application.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;

    public RefreshTokenService(
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IUserRepository userRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }
    
    private static string GenerateRandomToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var refreshToken = GenerateRandomToken();
        
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(DefaultCodes.RefreshTokenExpireDay),
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        return refreshToken;
    }

    public async Task<(string AccessToken, string RefreshToken)?> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken, true, cancellationToken);
        if (tokenEntity is not { IsActive: true })
            return null;

        var user = await _userRepository.GetByIdAsync(tokenEntity.UserId, false, cancellationToken);
        if (user == null || user.IsDeleted || !user.IsActive)
            return null;

        await _refreshTokenRepository.RevokeTokenAsync(refreshToken, "Replaced by new token", "System", cancellationToken);

        var userResponse = new DynamicERP.Core.ResponseModels.UserResponseModel
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            ProfilePictureUrl = user.ProfilePictureUrl,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt
        };

        var accessToken = _jwtService.GenerateToken(userResponse);
        var newRefreshToken = await GenerateRefreshTokenAsync(user.Id, cancellationToken);

        tokenEntity.ReplacedByToken = newRefreshToken;

        return (accessToken, newRefreshToken);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, string reason, string revokedBy, CancellationToken cancellationToken = default)
    {
        await _refreshTokenRepository.RevokeTokenAsync(refreshToken, reason, revokedBy, cancellationToken);
    }

    public async Task RevokeAllUserTokensAsync(Guid userId, string reason, string revokedBy, CancellationToken cancellationToken = default)
    {
        await _refreshTokenRepository.RevokeAllUserTokensAsync(userId, reason, revokedBy, cancellationToken);
    }

    public async Task<bool> IsTokenActiveAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _refreshTokenRepository.IsTokenActiveAsync(refreshToken, cancellationToken);
    }

} 