using System.Linq.Expressions;
using DynamicERP.Core.Entities;
using DynamicERP.Core.Interfaces.Repositories;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure.Repositories;

/// <summary>
/// Generic Repository implementasyonu.
/// </summary>
/// <typeparam name="TEntity">Entity tipi</typeparam>
/// <typeparam name="TKey">Entity'nin primary key tipi</typeparam>
public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IQueryable<TEntity>> GetAllAsync(bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        if (!isTracking)
            query = query.AsNoTracking();
        return query;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        if (!isTracking)
            query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, true, cancellationToken);
        if (entity != null)
            _dbSet.Remove(entity);
    }

    public virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public virtual async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(predicate);
        if (!isTracking)
            query = query.AsNoTracking();
        return query;
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        if (!isTracking)
            query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(predicate, cancellationToken);
    }

    public virtual async Task<bool> CancelAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, true, cancellationToken);
        if (entity == null)
            return false;

        if (entity is not BaseFullEntity<TKey> fullEntity)
            return false;

        fullEntity.IsActive = false;
        await UpdateAsync(entity, cancellationToken);
        return true;
    }

    public virtual async Task<bool> ActivateAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, true, cancellationToken);
        if (entity == null)
            return false;

        if (entity is not BaseFullEntity<TKey> fullEntity)
            return false;

        fullEntity.IsActive = true;
        await UpdateAsync(entity, cancellationToken);
        return true;
    }
} 