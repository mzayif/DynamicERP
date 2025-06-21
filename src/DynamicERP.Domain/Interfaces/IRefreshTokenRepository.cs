using DynamicERP.Domain.Entities;

namespace DynamicERP.Domain.Interfaces;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken, Guid>
{
    /// <summary>
    /// Token'� veritaban�ndan al�r.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="isTracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RefreshToken?> GetByTokenAsync(string token, bool isTracking = false, CancellationToken cancellationToken = default);
    /// <summary>
    /// Kullan�c� ID'sine g�re sadece aktif token'lar� d�ner.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Token'� iptal eder. <br/>
    /// RevokedAt g�ncellenir bilgileri ve IsActive = false yap�l�r<br/>
    /// Kay�tlar silmez, sadece iptal eder.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="reason"></param>
    /// <param name="revokedBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeTokenAsync(string token, string reason, string revokedBy, CancellationToken cancellationToken = default);
    /// <summary>
    /// Kullan�c�ya ait t�m token'lar� iptal eder<br/>
    /// (RevokedAt g�ncellenir ve IsActive = false yap�l�r).
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="reason"></param>
    /// <param name="revokedBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAllUserTokensAsync(Guid userId, string reason, string revokedBy, CancellationToken cancellationToken = default);
    /// <summary>
    /// Token'�n aktif olup olmad���n� kontrol eder.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsTokenActiveAsync(string token, CancellationToken cancellationToken = default);
} 