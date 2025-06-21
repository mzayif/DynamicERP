using System.ComponentModel.DataAnnotations;

namespace DynamicERP.Core.RequestModels;

/// <summary>
/// Kullanıcı girişi için request modeli
/// </summary>
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Beni hatırla (refresh token oluşturulsun mu?)
    /// </summary>
    public bool RememberMe { get; set; } = false;
} 