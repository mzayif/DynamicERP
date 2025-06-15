using DynamicERP.Core.Constants;
using DynamicERP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicERP.Infrastructure.Data.Configuration;

public class ExternalProviderConfiguration : IEntityTypeConfiguration<ExternalProvider>
{
    public void Configure(EntityTypeBuilder<ExternalProvider> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(ShortText.MaxLength);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(ShortText.MaxLength);

        builder.Property(e => e.Description)
            .HasMaxLength(MediumText.MaxLength);

        builder.Property(e => e.ClientId)
            .HasMaxLength(ShortText.MaxLength);

        builder.Property(e => e.ClientSecret)
            .HasMaxLength(ShortText.MaxLength);

        builder.Property(e => e.RedirectUri)
            .HasMaxLength(Url.MaxLength);

        builder.Property(e => e.AuthorizationEndpoint)
            .HasMaxLength(Url.MaxLength);

        builder.Property(e => e.TokenEndpoint)
            .HasMaxLength(Url.MaxLength);

        builder.Property(e => e.UserInfoEndpoint)
            .HasMaxLength(Url.MaxLength);

        builder.Property(e => e.Scope)
            .HasMaxLength(MediumText.MaxLength);

        builder.Property(e => e.IconUrl)
            .HasMaxLength(Url.MaxLength);

        builder.Property(e => e.BackgroundColor)
            .HasMaxLength(ShortText.MaxLength);

        builder.Property(e => e.TextColor)
            .HasMaxLength(ShortText.MaxLength);

        builder.HasIndex(e => e.Code)
            .IsUnique();
    }
} 