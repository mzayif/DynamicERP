using DynamicERP.Core.Entities.BaseClasses;
using System;

namespace DynamicERP.Core.Entities;

public class User : BaseTenantEntity
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PasswordHash { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? ExternalId { get; set; } // LDAP veya sosyal medya ID'si için
    public string? ExternalProvider { get; set; } // "LDAP", "Google", "Microsoft" vb.
    public string? PhoneNumber { get; set; }
    public string? ProfilePictureUrl { get; set; }

    // Navigation property
    public Tenant Tenant { get; set; } = null!;
} 