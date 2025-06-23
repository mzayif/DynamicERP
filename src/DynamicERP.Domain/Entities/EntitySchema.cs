using DynamicERP.Domain.Entities.BaseClasses;

namespace DynamicERP.Domain.Entities;

/// <summary>
/// Dinamik entity'lerin şema tanımlarını tutan entity
/// Bu entity hangi tabloların olduğunu ve özelliklerini saklar
/// Örnek: Customer, Product, Order, Employee gibi dinamik tablolar
/// </summary>
public class EntitySchema : BaseFullEntity
{
    /// <summary>
    /// Entity tipi (Customer, Product, Order, etc.)
    /// Bu alan unique olmalı çünkü her entity tipi sadece bir kez tanımlanabilir
    /// </summary>
    public string EntityType { get; set; } = null!;
    
    /// <summary>
    /// Entity'nin görünen adı (Müşteri, Ürün, Sipariş, etc.)
    /// Kullanıcı arayüzünde gösterilecek isim
    /// </summary>
    public string DisplayName { get; set; } = null!;
    
    /// <summary>
    /// Entity'nin açıklaması
    /// Ne işe yaradığını açıklayan metin
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Entity'nin hangi tenant'a ait olduğu
    /// Multi-tenant yapı için gerekli
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// Şema versiyonu
    /// Entity'ye alan eklendiğinde/çıkarıldığında versiyon artar
    /// Gelecekte migration için kullanılacak
    /// </summary>
    public int Version { get; set; } = 1;
    
    /// <summary>
    /// Entity'nin aktif olup olmadığı
    /// Pasif entity'ler kullanılamaz
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Entity'nin hangi tenant'a ait olduğu (navigation property)
    /// </summary>
    public Tenant Tenant { get; set; } = null!;
    
    /// <summary>
    /// Bu entity'ye ait alan tanımları (navigation property)
    /// Bir entity'nin birden fazla alanı olabilir
    /// </summary>
    public ICollection<FieldDefinition> Fields { get; set; } = new List<FieldDefinition>();
} 