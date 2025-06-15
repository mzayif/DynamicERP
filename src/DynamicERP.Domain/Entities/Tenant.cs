using DynamicERP.Domain.Entities.BaseClasses;

namespace DynamicERP.Domain.Entities;

public class Tenant : BaseFullEntity
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? Schema { get; set; }
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public string? LogoUrl { get; set; }
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? TaxNumber { get; set; }
    public string? TaxOffice { get; set; }
    public string? TimeZone { get; set; }
    public string? Language { get; set; }
    public string? Currency { get; set; }

    // Navigation property
    public ICollection<User> Users { get; set; } = new List<User>();
} 