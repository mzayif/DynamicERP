using DynamicERP.Domain.Entities.BaseClasses;

namespace DynamicERP.Domain.Entities;

public class UserType : BaseFullEntity
{
    public ICollection<User> Users { get; set; } = new List<User>();
} 