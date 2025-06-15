using System.Linq.Expressions;
using DynamicERP.Core.Entities.BaseClasses;

namespace DynamicERP.Core.Interfaces.Repositories;

/// <summary>
/// Tüm entity'ler için temel CRUD operasyonlarını sağlayan generic repository interface'i.
/// </summary>
/// <typeparam name="TEntity">Entity tipi</typeparam>
/// <typeparam name="TKey">Entity'nin primary key tipi</typeparam>
public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    /// <summary>
    /// Tüm kayıtları IQueryable olarak getirir.
    /// </summary>
    /// <param name="isTracking">Entity Framework change tracking'in aktif olup olmayacağı</param>
    IQueryable<TEntity> GetAll(bool isTracking = false);

    /// <summary>
    /// Belirtilen koşula göre kayıtları IQueryable olarak getirir.
    /// </summary>
    /// <param name="predicate">Filtreleme koşulu</param>
    /// <param name="isTracking">Entity Framework change tracking'in aktif olup olmayacağı</param>
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool isTracking = false);

    /// <summary>
    /// Belirtilen ID'ye sahip kaydı getirir.
    /// </summary>
    /// <param name="id">Kaydın ID'si</param>
    /// <param name="isTracking">Entity Framework change tracking'in aktif olup olmayacağı</param>
    /// <param name="cancellationToken">İşlemin iptal edilmesi için token</param>
    Task<TEntity?> GetByIdAsync(TKey id, bool isTracking = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir kayıt ekler.
    /// </summary>
    /// <param name="entity">Eklenecek entity</param>
    /// <param name="cancellationToken">İşlemin iptal edilmesi için token</param>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mevcut bir kaydı günceller.
    /// </summary>
    /// <param name="entity">Güncellenecek entity</param>
    /// <param name="cancellationToken">İşlemin iptal edilmesi için token</param>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen ID'ye sahip kaydı siler.
    /// </summary>
    /// <param name="id">Silinecek kaydın ID'si</param>
    /// <param name="cancellationToken">İşlemin iptal edilmesi için token</param>
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen ID'ye sahip kaydı iptal eder (IsActive = false).
    /// </summary>
    /// <param name="id">İptal edilecek kaydın ID'si</param>
    /// <param name="cancellationToken">İşlemin iptal edilmesi için token</param>
    Task<bool> CancelAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen ID'ye sahip iptal edilmiş kaydı aktif eder (IsActive = true).
    /// </summary>
    /// <param name="id">Aktif edilecek kaydın ID'si</param>
    /// <param name="cancellationToken">İşlemin iptal edilmesi için token</param>
    Task<bool> ActivateAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen ID'ye sahip kaydın var olup olmadığını kontrol eder.
    /// </summary>
    /// <param name="id">Kontrol edilecek kaydın ID'si</param>
    /// <param name="cancellationToken">İşlemin iptal edilmesi için token</param>
    Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen koşula göre ilk kaydı getirir.
    /// </summary>
    /// <param name="predicate">Filtreleme koşulu</param>
    /// <param name="isTracking">Entity Framework change tracking'in aktif olup olmayacağı</param>
    /// <param name="cancellationToken">İşlemin iptal edilmesi için token</param>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracking = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen koşula göre kayıtların sayısını getirir.
    /// </summary>
    /// <param name="predicate">Filtreleme koşulu</param>
    /// <param name="cancellationToken">İşlemin iptal edilmesi için token</param>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
} 