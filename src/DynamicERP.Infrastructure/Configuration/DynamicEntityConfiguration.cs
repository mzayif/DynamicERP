using DynamicERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicERP.Infrastructure.Configuration;

/// <summary>
/// DynamicEntity entity'si için Entity Framework configuration
/// Bu sınıf DynamicEntity tablosunun nasıl oluşturulacağını tanımlar
/// </summary>
public class DynamicEntityConfiguration : IEntityTypeConfiguration<DynamicEntity>
{
    public void Configure(EntityTypeBuilder<DynamicEntity> builder)
    {
        // Tablo adını belirle
        builder.ToTable("DynamicEntities");
        
        // Primary key tanımla
        builder.HasKey(x => x.Id);
        
        // SchemaId ve TenantId kombinasyonu için index
        // Performans için gerekli
        builder.HasIndex(x => new { x.SchemaId, x.TenantId })
            .HasDatabaseName("IX_DynamicEntities_SchemaId_TenantId");
        
        // Status alanı için index
        // Aktif/pasif verileri hızlı filtrelemek için
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_DynamicEntities_Status");
        
        // CreatedBy alanı için index
        // Kullanıcı bazlı sorgular için
        builder.HasIndex(x => x.CreatedBy)
            .HasDatabaseName("IX_DynamicEntities_CreatedBy");
        
        // SchemaId alanını zorunlu yap
        builder.Property(x => x.SchemaId)
            .IsRequired()
            .HasComment("Bu verinin hangi entity tipine ait olduğu");
        
        // TenantId alanını zorunlu yap
        builder.Property(x => x.TenantId)
            .IsRequired()
            .HasComment("Bu verinin hangi tenant'a ait olduğu");
        
        // Data alanını zorunlu yap
        // JSON formatında veri saklanacak
        builder.Property(x => x.Data)
            .IsRequired()
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("Verinin JSON formatında saklanan içeriği");
        
        // Status alanını zorunlu yap ve varsayılan değer belirle
        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Active")
            .HasComment("Verinin durumu (Active, Inactive, Deleted, etc.)");
        
        // CreatedBy alanını zorunlu yap
        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasComment("Bu veriyi oluşturan kullanıcı");
        
        // UpdatedBy alanını opsiyonel yap
        builder.Property(x => x.UpdatedBy)
            .IsRequired(false)
            .HasComment("Bu veriyi son güncelleyen kullanıcı");
        
        // Schema ile ilişki tanımla
        builder.HasOne(x => x.Schema)
            .WithMany()
            .HasForeignKey(x => x.SchemaId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_DynamicEntities_EntitySchemas");
        
        // Tenant ile ilişki tanımla
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_DynamicEntities_Tenants");
        
        // Creator ile ilişki tanımla
        builder.HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_DynamicEntities_Creator_Users");
        
        // Updater ile ilişki tanımla (opsiyonel)
        builder.HasOne(x => x.Updater)
            .WithMany()
            .HasForeignKey(x => x.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_DynamicEntities_Updater_Users");
        
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