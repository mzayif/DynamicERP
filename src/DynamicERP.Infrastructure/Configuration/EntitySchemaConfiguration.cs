using DynamicERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicERP.Infrastructure.Configuration;

/// <summary>
/// EntitySchema entity'si için Entity Framework configuration
/// Bu sınıf EntitySchema tablosunun nasıl oluşturulacağını tanımlar
/// </summary>
public class EntitySchemaConfiguration : IEntityTypeConfiguration<EntitySchema>
{
    public void Configure(EntityTypeBuilder<EntitySchema> builder)
    {
        // Tablo adını belirle
        builder.ToTable("EntitySchemas");
        
        // Primary key tanımla
        builder.HasKey(x => x.Id);
        
        // EntityType alanını unique yap (tenant bazında)
        // Aynı tenant'ta aynı entity tipi sadece bir kez olabilir
        builder.HasIndex(x => new { x.EntityType, x.TenantId })
            .IsUnique()
            .HasDatabaseName("IX_EntitySchemas_EntityType_TenantId");
        
        // EntityType alanını zorunlu yap ve maksimum uzunluk belirle
        builder.Property(x => x.EntityType)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Entity tipi (Customer, Product, Order, etc.)");
        
        // DisplayName alanını zorunlu yap ve maksimum uzunluk belirle
        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Entity'nin görünen adı (Müşteri, Ürün, Sipariş, etc.)");
        
        // Description alanını opsiyonel yap ve maksimum uzunluk belirle
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("Entity'nin açıklaması");
        
        // TenantId alanını zorunlu yap
        builder.Property(x => x.TenantId)
            .IsRequired()
            .HasComment("Entity'nin hangi tenant'a ait olduğu");
        
        // Version alanını varsayılan değerle tanımla
        builder.Property(x => x.Version)
            .IsRequired()
            .HasDefaultValue(1)
            .HasComment("Şema versiyonu");
        
        // IsActive alanını varsayılan değerle tanımla
        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Entity'nin aktif olup olmadığı");
        
        // Tenant ile ilişki tanımla
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_EntitySchemas_Tenants");
        
        // Fields ile ilişki tanımla (one-to-many)
        builder.HasMany(x => x.Fields)
            .WithOne(x => x.Schema)
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