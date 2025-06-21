namespace DynamicERP.Core.ResponseModels;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
} 