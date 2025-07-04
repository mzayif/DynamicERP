namespace DynamicERP.Core.ResponseModels;

/// <summary>
/// Kullanıcı girişi için response modeli
/// </summary>
public class LoginResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public UserResponseModel User { get; set; } = null!;
} 