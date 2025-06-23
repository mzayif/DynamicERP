using DynamicERP.Domain.Entities;
using DynamicERP.Domain.Interfaces;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure.Repositories;

/// <summary>
/// EntitySchema entity'si için repository implementation
/// Bu sınıf EntitySchema ile ilgili tüm veritabanı işlemlerini gerçekleştirir
/// </summary>
public class EntitySchemaRepository : GenericRepository<EntitySchema, Guid>, IEntitySchemaRepository
{
    public EntitySchemaRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Belirli bir tenant'a ait tüm entity şemalarını getirir
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Tenant'a ait entity şemaları</returns>
    public async Task<IEnumerable<EntitySchema>> GetByTenantIdAsync(Guid tenantId)
    {
        return await Context.EntitySchemas
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .Include(x => x.Fields) // Alan tanımlarını da getir
            .OrderBy(x => x.DisplayName)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir entity tipini tenant'a göre getirir
    /// </summary>
    /// <param name="entityType">Entity tipi (Customer, Product, etc.)</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Entity şeması</returns>
    public async Task<EntitySchema?> GetByEntityTypeAndTenantAsync(string entityType, Guid tenantId)
    {
        return await Context.EntitySchemas
            .Where(x => x.EntityType == entityType && x.TenantId == tenantId && !x.IsDeleted)
            .Include(x => x.Fields.OrderBy(f => f.OrderIndex)) // Alanları sıralı getir
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Entity tipinin belirli bir tenant'ta var olup olmadığını kontrol eder
    /// </summary>
    /// <param name="entityType">Entity tipi</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Var ise true, yok ise false</returns>
    public async Task<bool> ExistsAsync(string entityType, Guid tenantId)
    {
        return await Context.EntitySchemas
            .AnyAsync(x => x.EntityType == entityType && x.TenantId == tenantId && !x.IsDeleted);
    }

    /// <summary>
    /// Aktif entity şemalarını getirir
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Aktif entity şemaları</returns>
    public async Task<IEnumerable<EntitySchema>> GetActiveSchemasAsync(Guid tenantId)
    {
        return await Context.EntitySchemas
            .Where(x => x.TenantId == tenantId && x.IsActive && !x.IsDeleted)
            .Include(x => x.Fields.Where(f => !f.IsDeleted)) // Sadece aktif alanları getir
            .OrderBy(x => x.DisplayName)
            .ToListAsync();
    }
} 