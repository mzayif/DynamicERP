using DynamicERP.Core.Entities.BaseClasses;

namespace DynamicERP.Core.Entities;

public class UserType : BaseFullEntity
{
    public ICollection<User> Users { get; set; } = new List<User>();
} 