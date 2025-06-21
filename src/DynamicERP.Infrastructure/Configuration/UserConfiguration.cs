using DynamicERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicERP.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.PasswordHash)
            .HasMaxLength(100);

        builder.Property(e => e.FirstName)
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .HasMaxLength(50);

        builder.Property(e => e.ExternalId)
            .HasMaxLength(100);

        builder.Property(e => e.ExternalProvider)
            .HasMaxLength(50);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.ProfilePictureUrl)
            .HasMaxLength(200);

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.HasIndex(e => e.Username)
            .IsUnique();

        builder.HasIndex(e => new { e.ExternalId, e.ExternalProvider })
            .IsUnique();

        builder.HasOne(e => e.Tenant)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}