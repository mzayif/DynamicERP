using DynamicERP.Domain.Entities;
using DynamicERP.Domain.Interfaces;
using DynamicERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace DynamicERP.Infrastructure.Repositories;

/// <summary>
/// DynamicEntity entity'si için repository implementation
/// Bu sınıf DynamicEntity ile ilgili tüm veritabanı işlemlerini gerçekleştirir
/// </summary>
public class DynamicEntityRepository : GenericRepository<DynamicEntity, Guid>, IDynamicEntityRepository
{
    public DynamicEntityRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Belirli bir entity tipine ait tüm verileri getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Entity'ye ait veriler</returns>
    public async Task<IEnumerable<DynamicEntity>> GetBySchemaAndTenantAsync(Guid schemaId, Guid tenantId)
    {
        return await DbSet.Where(x => x.SchemaId == schemaId && x.TenantId == tenantId && !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir entity tipine ait aktif verileri getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Aktif veriler</returns>
    public async Task<IEnumerable<DynamicEntity>> GetActiveBySchemaAndTenantAsync(Guid schemaId, Guid tenantId)
    {
        return await DbSet.Where(x => x.SchemaId == schemaId && x.TenantId == tenantId && x.Status == "Active" && !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir entity tipine ait verileri sayfalı şekilde getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="page">Sayfa numarası</param>
    /// <param name="pageSize">Sayfa boyutu</param>
    /// <returns>Sayfalı veriler</returns>
    public async Task<(IEnumerable<DynamicEntity> Data, int TotalCount)> GetPagedBySchemaAndTenantAsync(
        Guid schemaId, Guid tenantId, int page, int pageSize)
    {
        var query = DbSet.Where(x => x.SchemaId == schemaId && x.TenantId == tenantId && !x.IsDeleted);

        var totalCount = await query.CountAsync();
        
        var data = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalCount);
    }

    /// <summary>
    /// JSON data içinde arama yapar
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="searchTerm">Arama terimi</param>
    /// <param name="searchFields">Aranacak alanlar</param>
    /// <returns>Arama sonuçları</returns>
    public async Task<IEnumerable<DynamicEntity>> SearchAsync(
        Guid schemaId, Guid tenantId, string searchTerm, IEnumerable<string> searchFields)
    {
        var searchConditions = searchFields.Select(field => 
            $"JSON_VALUE(Data, '$.{field}') LIKE '%{searchTerm}%'");
        
        var whereClause = string.Join(" OR ", searchConditions);
        
        var sql = $@"
            SELECT * FROM DynamicEntities 
            WHERE SchemaId = @schemaId 
            AND TenantId = @tenantId 
            AND IsDeleted = 0 
            AND ({whereClause})
            ORDER BY CreatedAt DESC";
        
        return await DbSet.FromSqlRaw(sql, 
            new Microsoft.Data.SqlClient.SqlParameter("@schemaId", schemaId),
            new Microsoft.Data.SqlClient.SqlParameter("@tenantId", tenantId))
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir alan değerine göre veri getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="fieldName">Alan adı</param>
    /// <param name="fieldValue">Alan değeri</param>
    /// <returns>Eşleşen veriler</returns>
    public async Task<IEnumerable<DynamicEntity>> GetByFieldValueAsync(
        Guid schemaId, Guid tenantId, string fieldName, string fieldValue)
    {
        var sql = @"
            SELECT * FROM DynamicEntities 
            WHERE SchemaId = @schemaId 
            AND TenantId = @tenantId 
            AND IsDeleted = 0 
            AND JSON_VALUE(Data, @fieldPath) LIKE @fieldValue
            ORDER BY CreatedAt DESC";
        
        return await DbSet.FromSqlRaw(sql,
            new Microsoft.Data.SqlClient.SqlParameter("@schemaId", schemaId),
            new Microsoft.Data.SqlClient.SqlParameter("@tenantId", tenantId),
            new Microsoft.Data.SqlClient.SqlParameter("@fieldPath", "$." + fieldName),
            new Microsoft.Data.SqlClient.SqlParameter("@fieldValue", "%" + fieldValue + "%"))
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir kullanıcının oluşturduğu verileri getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <returns>Kullanıcının oluşturduğu veriler</returns>
    public async Task<IEnumerable<DynamicEntity>> GetByCreatorAsync(
        Guid schemaId, Guid tenantId, Guid userId)
    {
        return await DbSet
            .Where(x => x.SchemaId == schemaId && x.TenantId == tenantId && x.CreatedBy == userId && !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir tarih aralığındaki verileri getirir
    /// </summary>
    /// <param name="schemaId">Entity şema ID'si</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <returns>Tarih aralığındaki veriler</returns>
    public async Task<IEnumerable<DynamicEntity>> GetByDateRangeAsync(
        Guid schemaId, Guid tenantId, DateTime startDate, DateTime endDate)
    {
        return await DbSet
            .Where(x => x.SchemaId == schemaId && 
                       x.TenantId == tenantId && 
                       x.CreatedAt >= startDate && 
                       x.CreatedAt <= endDate && 
                       !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
} 