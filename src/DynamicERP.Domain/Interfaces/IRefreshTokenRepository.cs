using DynamicERP.Domain.Entities;

namespace DynamicERP.Domain.Interfaces;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken, Guid>
{
    /// <summary>
    /// Token'ý veritabanýndan alýr.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="isTracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RefreshToken?> GetByTokenAsync(string token, bool isTracking = false, CancellationToken cancellationToken = default);
    /// <summary>
    /// Kullanýcý ID'sine göre sadece aktif token'larý döner.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Token'ý iptal eder. <br/>
    /// RevokedAt güncellenir bilgileri ve IsActive = false yapýlýr<br/>
    /// Kayýtlar silmez, sadece iptal eder.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="reason"></param>
    /// <param name="revokedBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeTokenAsync(string token, string reason, string revokedBy, CancellationToken cancellationToken = default);
    /// <summary>
    /// Kullanýcýya ait tüm token'larý iptal eder<br/>
    /// (RevokedAt güncellenir ve IsActive = false yapýlýr).
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="reason"></param>
    /// <param name="revokedBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAllUserTokensAsync(Guid userId, string reason, string revokedBy, CancellationToken cancellationToken = default);
    /// <summary>
    /// Token'ýn aktif olup olmadýðýný kontrol eder.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsTokenActiveAsync(string token, CancellationToken cancellationToken = default);
} 