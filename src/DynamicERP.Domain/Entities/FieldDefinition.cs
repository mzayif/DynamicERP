using DynamicERP.Domain.Entities.BaseClasses;

namespace DynamicERP.Domain.Entities;

/// <summary>
/// Dinamik entity'lerin alan tanımlarını tutan entity
/// Bu entity her tablonun hangi alanlara sahip olduğunu saklar
/// Örnek: Customer tablosunda Name, Email, Phone alanları
/// </summary>
public class FieldDefinition : BaseFullEntity
{
    /// <summary>
    /// Bu alanın hangi entity'ye ait olduğu
    /// Foreign key to EntitySchema
    /// </summary>
    public Guid SchemaId { get; set; }
    
    /// <summary>
    /// Alan adı (Name, Email, Phone, etc.)
    /// Veritabanında ve kodda kullanılacak isim
    /// </summary>
    public string FieldName { get; set; } = null!;
    
    /// <summary>
    /// Alanın görünen adı (Müşteri Adı, E-posta, Telefon, etc.)
    /// Kullanıcı arayüzünde gösterilecek isim
    /// </summary>
    public string DisplayName { get; set; } = null!;
    
    /// <summary>
    /// Alan tipi (Text, Number, Date, Boolean, Dropdown, etc.)
    /// Form render edilirken hangi input tipi kullanılacağını belirler
    /// </summary>
    public string FieldType { get; set; } = null!;
    
    /// <summary>
    /// Veri tipi (string, int, datetime, bool, etc.)
    /// Veritabanında nasıl saklanacağını belirler
    /// </summary>
    public string DataType { get; set; } = null!;
    
    /// <summary>
    /// Alan zorunlu mu?
    /// true ise bu alan boş bırakılamaz
    /// </summary>
    public bool IsRequired { get; set; } = false;
    
    /// <summary>
    /// Alan aranabilir mi?
    /// true ise bu alan üzerinde arama yapılabilir
    /// </summary>
    public bool IsSearchable { get; set; } = false;
    
    /// <summary>
    /// Alan sıralanabilir mi?
    /// true ise bu alana göre sıralama yapılabilir
    /// </summary>
    public bool IsSortable { get; set; } = false;
    
    /// <summary>
    /// Alanın varsayılan değeri
    /// Yeni kayıt oluşturulurken otomatik doldurulacak değer
    /// </summary>
    public string? DefaultValue { get; set; }
    
    /// <summary>
    /// Alanın maksimum uzunluğu
    /// Text alanları için karakter sınırı
    /// </summary>
    public int? MaxLength { get; set; }
    
    /// <summary>
    /// Alanın minimum uzunluğu
    /// Text alanları için minimum karakter sayısı
    /// </summary>
    public int? MinLength { get; set; }
    
    /// <summary>
    /// Alanın maksimum değeri
    /// Number alanları için üst sınır
    /// </summary>
    public decimal? MaxValue { get; set; }
    
    /// <summary>
    /// Alanın minimum değeri
    /// Number alanları için alt sınır
    /// </summary>
    public decimal? MinValue { get; set; }
    
    /// <summary>
    /// Dropdown alanları için seçenekler (JSON formatında)
    /// Örnek: ["VIP", "Regular", "Premium"]
    /// </summary>
    public string? Options { get; set; }
    
    /// <summary>
    /// Validation kuralları (JSON formatında)
    /// Gelecekte plugin sistemi için hazır
    /// Örnek: {"pattern": "^[^@]+@[^@]+\\.[^@]+$", "message": "Geçerli email giriniz"}
    /// </summary>
    public string? ValidationRules { get; set; }
    
    /// <summary>
    /// Alanın sıralama indeksi
    /// Form ve listelerde hangi sırada gösterileceğini belirler
    /// </summary>
    public int OrderIndex { get; set; } = 0;
    
    /// <summary>
    /// Alanın hangi entity'ye ait olduğu (navigation property)
    /// </summary>
    public EntitySchema Schema { get; set; } = null!;
} 