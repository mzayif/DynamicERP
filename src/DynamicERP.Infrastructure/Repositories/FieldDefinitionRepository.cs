using DynamicERP.Domain.Entities;
using DynamicERP.Domain.Interfaces;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Infrastructure.Repositories;

/// <summary>
/// FieldDefinition entity'si için repository implementation
/// Bu sınıf FieldDefinition ile ilgili tüm veritabanı işlemlerini gerçekleştirir
/// </summary>
public class FieldDefinitionRepository : GenericRepository<FieldDefinition, Guid>, IFieldDefinitionRepository
{
    public FieldDefinitionRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Belirli bir entity şemasına ait tüm alanları getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <returns>Şemaya ait alan tanımları</returns>
    public async Task<IEnumerable<FieldDefinition>> GetBySchemaIdAsync(Guid schemaId)
    {
        return await Context.FieldDefinitions
            .Where(x => x.SchemaId == schemaId && !x.IsDeleted)
            .OrderBy(x => x.OrderIndex)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir entity şemasına ait alanları sıralı şekilde getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <returns>Sıralı alan tanımları</returns>
    public async Task<IEnumerable<FieldDefinition>> GetBySchemaIdOrderedAsync(Guid schemaId)
    {
        return await Context.FieldDefinitions
            .Where(x => x.SchemaId == schemaId && !x.IsDeleted)
            .OrderBy(x => x.OrderIndex)
            .ThenBy(x => x.DisplayName)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir alan adının şemada var olup olmadığını kontrol eder
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="fieldName">Alan adı</param>
    /// <returns>Var ise true, yok ise false</returns>
    public async Task<bool> FieldExistsAsync(Guid schemaId, string fieldName)
    {
        return await Context.FieldDefinitions
            .AnyAsync(x => x.SchemaId == schemaId && x.FieldName == fieldName && !x.IsDeleted);
    }

    /// <summary>
    /// Belirli bir entity tipine ait tüm alanları getirir
    /// </summary>
    /// <param name="entityType">Entity tipi (Customer, Product, etc.)</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Entity'ye ait alan tanımları</returns>
    public async Task<IEnumerable<FieldDefinition>> GetByEntityTypeAndTenantAsync(string entityType, Guid tenantId)
    {
        return await Context.FieldDefinitions
            .Where(x => x.Schema.EntityType == entityType && 
                       x.Schema.TenantId == tenantId && 
                       !x.IsDeleted && 
                       !x.Schema.IsDeleted)
            .OrderBy(x => x.OrderIndex)
            .ThenBy(x => x.DisplayName)
            .ToListAsync();
    }

    /// <summary>
    /// Aranabilir alanları getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <returns>Aranabilir alan tanımları</returns>
    public async Task<IEnumerable<FieldDefinition>> GetSearchableFieldsAsync(Guid schemaId)
    {
        return await Context.FieldDefinitions
            .Where(x => x.SchemaId == schemaId && x.IsSearchable && !x.IsDeleted)
            .OrderBy(x => x.OrderIndex)
            .ToListAsync();
    }

    /// <summary>
    /// Sıralanabilir alanları getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <returns>Sıralanabilir alan tanımları</returns>
    public async Task<IEnumerable<FieldDefinition>> GetSortableFieldsAsync(Guid schemaId)
    {
        return await Context.FieldDefinitions
            .Where(x => x.SchemaId == schemaId && x.IsSortable && !x.IsDeleted)
            .OrderBy(x => x.OrderIndex)
            .ToListAsync();
    }
} 