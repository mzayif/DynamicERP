using System.Linq.Expressions;
using DynamicERP.Core.Entities.BaseClasses;
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

    public virtual IQueryable<TEntity> GetAll(bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        if (!isTracking)
        {
            query = query.AsNoTracking();
        }
        return query;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        if (!isTracking)
        {
            query = query.AsNoTracking();
        }
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

    public virtual async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, true, cancellationToken);
        if (entity == null)
            return false;

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(predicate);
        if (!isTracking)
        {
            query = query.AsNoTracking();
        }
        return query;
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(predicate);
        if (!isTracking)
        {
            query = query.AsNoTracking();
        }
        return await query.FirstOrDefaultAsync(cancellationToken);
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

        entity.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public virtual async Task<bool> ActivateAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, true, cancellationToken);
        if (entity == null)
            return false;

        entity.IsActive = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
} 