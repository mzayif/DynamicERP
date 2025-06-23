using DynamicERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicERP.Infrastructure.Configuration;

/// <summary>
/// FieldDefinition entity'si için Entity Framework configuration
/// Bu sınıf FieldDefinition tablosunun nasıl oluşturulacağını tanımlar
/// </summary>
public class FieldDefinitionConfiguration : IEntityTypeConfiguration<FieldDefinition>
{
    public void Configure(EntityTypeBuilder<FieldDefinition> builder)
    {
        // Tablo adını belirle
        builder.ToTable("FieldDefinitions");
        
        // Primary key tanımla
        builder.HasKey(x => x.Id);
        
        // SchemaId ve FieldName kombinasyonunu unique yap
        // Aynı şemada aynı alan adı sadece bir kez olabilir
        builder.HasIndex(x => new { x.SchemaId, x.FieldName })
            .IsUnique()
            .HasDatabaseName("IX_FieldDefinitions_SchemaId_FieldName");
        
        // SchemaId alanını zorunlu yap
        builder.Property(x => x.SchemaId)
            .IsRequired()
            .HasComment("Bu alanın hangi entity'ye ait olduğu");
        
        // FieldName alanını zorunlu yap ve maksimum uzunluk belirle
        builder.Property(x => x.FieldName)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Alan adı (Name, Email, Phone, etc.)");
        
        // DisplayName alanını zorunlu yap ve maksimum uzunluk belirle
        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Alanın görünen adı (Müşteri Adı, E-posta, Telefon, etc.)");
        
        // FieldType alanını zorunlu yap ve maksimum uzunluk belirle
        builder.Property(x => x.FieldType)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Alan tipi (Text, Number, Date, Boolean, Dropdown, etc.)");
        
        // DataType alanını zorunlu yap ve maksimum uzunluk belirle
        builder.Property(x => x.DataType)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Veri tipi (string, int, datetime, bool, etc.)");
        
        // Boolean alanları varsayılan değerlerle tanımla
        builder.Property(x => x.IsRequired)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Alan zorunlu mu?");
        
        builder.Property(x => x.IsSearchable)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Alan aranabilir mi?");
        
        builder.Property(x => x.IsSortable)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Alan sıralanabilir mi?");
        
        // Opsiyonel alanları tanımla
        builder.Property(x => x.DefaultValue)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("Alanın varsayılan değeri");
        
        builder.Property(x => x.MaxLength)
            .IsRequired(false)
            .HasComment("Alanın maksimum uzunluğu");
        
        builder.Property(x => x.MinLength)
            .IsRequired(false)
            .HasComment("Alanın minimum uzunluğu");
        
        builder.Property(x => x.MaxValue)
            .IsRequired(false)
            .HasPrecision(18, 2)
            .HasComment("Alanın maksimum değeri");
        
        builder.Property(x => x.MinValue)
            .IsRequired(false)
            .HasPrecision(18, 2)
            .HasComment("Alanın minimum değeri");
        
        // JSON alanları için maksimum uzunluk belirle
        builder.Property(x => x.Options)
            .IsRequired(false)
            .HasMaxLength(2000)
            .HasComment("Dropdown alanları için seçenekler (JSON formatında)");
        
        builder.Property(x => x.ValidationRules)
            .IsRequired(false)
            .HasMaxLength(2000)
            .HasComment("Validation kuralları (JSON formatında)");
        
        // OrderIndex alanını varsayılan değerle tanımla
        builder.Property(x => x.OrderIndex)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Alanın sıralama indeksi");
        
        // Schema ile ilişki tanımla
        builder.HasOne(x => x.Schema)
            .WithMany(x => x.Fields)
            .HasForeignKey(x => x.SchemaId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_FieldDefinitions_EntitySchemas");
        
        // Audit alanları için varsayılan değerler
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasComment("Oluşturulma tarihi");
        
        builder.Property(x => x.UpdatedAt)
            .IsRequired(false)
            .HasComment("Güncellenme tarihi");
    }
} 