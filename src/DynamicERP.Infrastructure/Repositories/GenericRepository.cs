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
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }
    
    public IQueryable<TEntity> GetAll(bool isTracking = false)
    {
        var query = DbSet.AsQueryable();
        if (!isTracking)
            query = query.AsNoTracking();
        return query;
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool isTracking = false)
    {
        var query = DbSet.Where(predicate);
        if (!isTracking)
            query = query.AsNoTracking();
        return query;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();
        if (!isTracking)
            query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, true, cancellationToken);
        if (entity != null)
            DbSet.Remove(entity);
    }

    public virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(e => e.Id!.Equals(id), cancellationToken);
    }
    
    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();
        if (!isTracking)
            query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.CountAsync(predicate, cancellationToken);
    }

    public virtual async Task<bool> CancelAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, true, cancellationToken);
        if (entity is not BaseFullEntity fullEntity)
            return false;

        fullEntity.IsActive = false;
        await UpdateAsync(entity, cancellationToken);
        return true;
    }

    public virtual async Task<bool> ActivateAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, true, cancellationToken);
        if (entity is not BaseFullEntity fullEntity)
            return false;

        fullEntity.IsActive = true;
        await UpdateAsync(entity, cancellationToken);
        return true;
    }
} 