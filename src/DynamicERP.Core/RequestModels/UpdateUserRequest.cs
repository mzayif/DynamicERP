namespace DynamicERP.Core.RequestModels;

public class UpdateUserRequest
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? ProfilePictureUrl { get; set; }
}