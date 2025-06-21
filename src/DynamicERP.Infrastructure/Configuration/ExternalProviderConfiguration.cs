using DynamicERP.Core.Constants;
using DynamicERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicERP.Infrastructure.Configuration;

public class ExternalProviderConfiguration : IEntityTypeConfiguration<ExternalProvider>
{
    public void Configure(EntityTypeBuilder<ExternalProvider> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(ValidationRules.ShortText.MaxLength);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(ValidationRules.ShortText.MaxLength);

        builder.Property(e => e.Description)
            .HasMaxLength(ValidationRules.MediumText.MaxLength);

        builder.Property(e => e.ClientId)
            .HasMaxLength(ValidationRules.ShortText.MaxLength);

        builder.Property(e => e.ClientSecret)
            .HasMaxLength(ValidationRules.ShortText.MaxLength);

        builder.Property(e => e.RedirectUri)
            .HasMaxLength(ValidationRules.Url.MaxLength);

        builder.Property(e => e.AuthorizationEndpoint)
            .HasMaxLength(ValidationRules.Url.MaxLength);

        builder.Property(e => e.TokenEndpoint)
            .HasMaxLength(ValidationRules.Url.MaxLength);

        builder.Property(e => e.UserInfoEndpoint)
            .HasMaxLength(ValidationRules.Url.MaxLength);

        builder.Property(e => e.Scope)
            .HasMaxLength(ValidationRules.MediumText.MaxLength);

        builder.Property(e => e.IconUrl)
            .HasMaxLength(ValidationRules.Url.MaxLength);

        builder.Property(e => e.BackgroundColor)
            .HasMaxLength(ValidationRules.ShortText.MaxLength);

        builder.Property(e => e.TextColor)
            .HasMaxLength(ValidationRules.ShortText.MaxLength);

        builder.HasIndex(e => e.Code)
            .IsUnique();
    }
}