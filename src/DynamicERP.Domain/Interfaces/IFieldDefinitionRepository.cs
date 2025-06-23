using DynamicERP.Domain.Entities;

namespace DynamicERP.Domain.Interfaces;

/// <summary>
/// FieldDefinition entity'si için repository interface'i
/// Bu interface FieldDefinition ile ilgili tüm veritabanı işlemlerini tanımlar
/// </summary>
public interface IFieldDefinitionRepository : IGenericRepository<FieldDefinition, Guid>
{
    /// <summary>
    /// Belirli bir entity şemasına ait tüm alanları getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <returns>Şemaya ait alan tanımları</returns>
    Task<IEnumerable<FieldDefinition>> GetBySchemaIdAsync(Guid schemaId);
    
    /// <summary>
    /// Belirli bir entity şemasına ait alanları sıralı şekilde getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <returns>Sıralı alan tanımları</returns>
    Task<IEnumerable<FieldDefinition>> GetBySchemaIdOrderedAsync(Guid schemaId);
    
    /// <summary>
    /// Belirli bir alan adının şemada var olup olmadığını kontrol eder
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="fieldName">Alan adı</param>
    /// <returns>Var ise true, yok ise false</returns>
    Task<bool> FieldExistsAsync(Guid schemaId, string fieldName);
    
    /// <summary>
    /// Belirli bir entity tipine ait tüm alanları getirir
    /// </summary>
    /// <param name="entityType">Entity tipi (Customer, Product, etc.)</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Entity'ye ait alan tanımları</returns>
    Task<IEnumerable<FieldDefinition>> GetByEntityTypeAndTenantAsync(string entityType, Guid tenantId);
    
    /// <summary>
    /// Aranabilir alanları getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <returns>Aranabilir alan tanımları</returns>
    Task<IEnumerable<FieldDefinition>> GetSearchableFieldsAsync(Guid schemaId);
    
    /// <summary>
    /// Sıralanabilir alanları getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <returns>Sıralanabilir alan tanımları</returns>
    Task<IEnumerable<FieldDefinition>> GetSortableFieldsAsync(Guid schemaId);
} 