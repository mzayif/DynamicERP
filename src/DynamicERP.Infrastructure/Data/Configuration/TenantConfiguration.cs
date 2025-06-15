using DynamicERP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicERP.Infrastructure.Data.Configuration;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.ConnectionString)
            .HasMaxLength(500);

        builder.Property(e => e.DatabaseName)
            .HasMaxLength(100);

        builder.Property(e => e.Schema)
            .HasMaxLength(50);

        builder.Property(e => e.LogoUrl)
            .HasMaxLength(200);

        builder.Property(e => e.Website)
            .HasMaxLength(200);

        builder.Property(e => e.Email)
            .HasMaxLength(100);

        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.Property(e => e.Address)
            .HasMaxLength(500);

        builder.Property(e => e.TaxNumber)
            .HasMaxLength(20);

        builder.Property(e => e.TaxOffice)
            .HasMaxLength(100);

        builder.Property(e => e.TimeZone)
            .HasMaxLength(50);

        builder.Property(e => e.Language)
            .HasMaxLength(10);

        builder.Property(e => e.Currency)
            .HasMaxLength(10);

        builder.HasIndex(e => e.Code)
            .IsUnique();
    }
} 