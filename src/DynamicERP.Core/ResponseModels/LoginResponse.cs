namespace DynamicERP.Core.ResponseModels;

/// <summary>
/// Kullanıcı girişi için response modeli
/// </summary>
public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public int ExpiresInMinutes { get; set; }
    public UserResponseModel User { get; set; } = new();
} 