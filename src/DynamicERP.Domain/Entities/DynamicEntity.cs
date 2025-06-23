using DynamicERP.Domain.Entities.BaseClasses;

namespace DynamicERP.Domain.Entities;

/// <summary>
/// Dinamik entity'lerin gerçek verilerini tutan entity
/// Bu entity tüm dinamik verileri JSON formatında saklar
/// Örnek: Ahmet Yılmaz'ın tüm müşteri bilgileri JSON olarak
/// </summary>
public class DynamicEntity : BaseFullEntity
{
    /// <summary>
    /// Bu verinin hangi entity tipine ait olduğu
    /// Foreign key to EntitySchema
    /// </summary>
    public Guid SchemaId { get; set; }
    
    /// <summary>
    /// Bu verinin hangi tenant'a ait olduğu
    /// Multi-tenant yapı için gerekli
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// Verinin JSON formatında saklanan içeriği
    /// Örnek: {"Name": "Ahmet Yılmaz", "Email": "ahmet@email.com", "Phone": "5551234567"}
    /// Bu alan tüm dinamik alanları içerir
    /// </summary>
    public string Data { get; set; } = null!;
    
    /// <summary>
    /// Verinin durumu (Active, Inactive, Deleted, etc.)
    /// Soft delete ve durum yönetimi için
    /// </summary>
    public string Status { get; set; } = "Active";
    
    /// <summary>
    /// Bu veriyi oluşturan kullanıcı
    /// Audit trail için gerekli
    /// </summary>
    public Guid CreatedBy { get; set; }
    
    /// <summary>
    /// Bu veriyi son güncelleyen kullanıcı
    /// Audit trail için gerekli
    /// </summary>
    public Guid? UpdatedBy { get; set; }
    
    /// <summary>
    /// Bu verinin hangi entity tipine ait olduğu (navigation property)
    /// </summary>
    public EntitySchema Schema { get; set; } = null!;
    
    /// <summary>
    /// Bu verinin hangi tenant'a ait olduğu (navigation property)
    /// </summary>
    public Tenant Tenant { get; set; } = null!;
    
    /// <summary>
    /// Bu veriyi oluşturan kullanıcı (navigation property)
    /// </summary>
    public User Creator { get; set; } = null!;
    
    /// <summary>
    /// Bu veriyi son güncelleyen kullanıcı (navigation property)
    /// </summary>
    public User? Updater { get; set; }
} 