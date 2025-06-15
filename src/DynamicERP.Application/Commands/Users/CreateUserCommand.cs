using DynamicERP.Core.Entities;
using DynamicERP.Core.Interfaces.CQRS;

namespace DynamicERP.Application.Commands.Users;

public class CreateUserCommand : ICommand<User>
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ExternalId { get; set; }
    public string? ExternalProvider { get; set; }
} 