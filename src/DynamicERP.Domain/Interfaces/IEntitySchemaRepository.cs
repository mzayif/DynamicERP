using DynamicERP.Domain.Entities;

namespace DynamicERP.Domain.Interfaces;

/// <summary>
/// EntitySchema entity'si için repository interface'i
/// Bu interface EntitySchema ile ilgili tüm veritabanı işlemlerini tanımlar
/// </summary>
public interface IEntitySchemaRepository : IGenericRepository<EntitySchema, Guid>
{
    /// <summary>
    /// Belirli bir tenant'a ait tüm entity şemalarını getirir
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Tenant'a ait entity şemaları</returns>
    Task<IEnumerable<EntitySchema>> GetByTenantIdAsync(Guid tenantId);
    
    /// <summary>
    /// Belirli bir entity tipini tenant'a göre getirir
    /// </summary>
    /// <param name="entityType">Entity tipi (Customer, Product, etc.)</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Entity şeması</returns>
    Task<EntitySchema?> GetByEntityTypeAndTenantAsync(string entityType, Guid tenantId);
    
    /// <summary>
    /// Entity tipinin belirli bir tenant'ta var olup olmadığını kontrol eder
    /// </summary>
    /// <param name="entityType">Entity tipi</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Var ise true, yok ise false</returns>
    Task<bool> ExistsAsync(string entityType, Guid tenantId);
    
    /// <summary>
    /// Aktif entity şemalarını getirir
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Aktif entity şemaları</returns>
    Task<IEnumerable<EntitySchema>> GetActiveSchemasAsync(Guid tenantId);
} 