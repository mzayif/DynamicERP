using DynamicERP.Domain.Entities;

namespace DynamicERP.Domain.Interfaces;

/// <summary>
/// DynamicEntity entity'si için repository interface'i
/// Bu interface DynamicEntity ile ilgili tüm veritabanı işlemlerini tanımlar
/// </summary>
public interface IDynamicEntityRepository : IGenericRepository<DynamicEntity, Guid>
{
    /// <summary>
    /// Belirli bir entity tipine ait tüm verileri getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Entity'ye ait veriler</returns>
    Task<IEnumerable<DynamicEntity>> GetBySchemaAndTenantAsync(Guid schemaId, Guid tenantId);
    
    /// <summary>
    /// Belirli bir entity tipine ait aktif verileri getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Aktif veriler</returns>
    Task<IEnumerable<DynamicEntity>> GetActiveBySchemaAndTenantAsync(Guid schemaId, Guid tenantId);
    
    /// <summary>
    /// Belirli bir entity tipine ait verileri sayfalı şekilde getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="page">Sayfa numarası</param>
    /// <param name="pageSize">Sayfa boyutu</param>
    /// <returns>Sayfalı veriler</returns>
    Task<(IEnumerable<DynamicEntity> Data, int TotalCount)> GetPagedBySchemaAndTenantAsync(
        Guid schemaId, Guid tenantId, int page, int pageSize);
    
    /// <summary>
    /// JSON data içinde arama yapar
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="searchTerm">Arama terimi</param>
    /// <param name="searchFields">Aranacak alanlar</param>
    /// <returns>Arama sonuçları</returns>
    Task<IEnumerable<DynamicEntity>> SearchAsync(
        Guid schemaId, Guid tenantId, string searchTerm, IEnumerable<string> searchFields);
    
    /// <summary>
    /// Belirli bir alan değerine göre veri getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="fieldName">Alan adı</param>
    /// <param name="fieldValue">Alan değeri</param>
    /// <returns>Eşleşen veriler</returns>
    Task<IEnumerable<DynamicEntity>> GetByFieldValueAsync(
        Guid schemaId, Guid tenantId, string fieldName, string fieldValue);
    
    /// <summary>
    /// Belirli bir kullanıcının oluşturduğu verileri getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <returns>Kullanıcının oluşturduğu veriler</returns>
    Task<IEnumerable<DynamicEntity>> GetByCreatorAsync(
        Guid schemaId, Guid tenantId, Guid userId);
    
    /// <summary>
    /// Belirli bir tarih aralığındaki verileri getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <returns>Tarih aralığındaki veriler</returns>
    Task<IEnumerable<DynamicEntity>> GetByDateRangeAsync(
        Guid schemaId, Guid tenantId, DateTime startDate, DateTime endDate);
} 