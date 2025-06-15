using DynamicERP.Domain.Entities.BaseClasses;

namespace DynamicERP.Domain.Entities;

/// <summary>
/// Harici kimlik doğrulama sağlayıcılarını yönetmek için kullanılan entity.
/// 
/// Özellikler:
/// - Name: Sağlayıcının görünen adı (örn: "Google", "Microsoft")
/// - Code: Sağlayıcının benzersiz kodu (örn: "GOOGLE", "MICROSOFT")
/// - Description: Sağlayıcı hakkında açıklama
/// 
/// OAuth Konfigürasyonu:
/// - ClientId: OAuth sağlayıcıları için client ID
/// - ClientSecret: OAuth sağlayıcıları için client secret
/// - RedirectUri: Kimlik doğrulama sonrası yönlendirilecek URI
/// - AuthorizationEndpoint: OAuth authorization endpoint URL'i
/// - TokenEndpoint: OAuth token endpoint URL'i
/// - UserInfoEndpoint: OAuth user info endpoint URL'i
/// - Scope: OAuth scope değerleri
/// 
/// UI Özelleştirme:
/// - IconUrl: Sağlayıcının ikon URL'i
/// - BackgroundColor: Login butonunun arka plan rengi
/// - TextColor: Login butonunun metin rengi
/// - DisplayOrder: Login butonlarının sıralama değeri
/// 
/// Durum Yönetimi:
/// - IsActive: Sağlayıcının aktif/pasif durumu (BaseFullEntity'den gelir)
/// </summary>
public class ExternalProvider : BaseFullEntity
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? RedirectUri { get; set; }
    public string? AuthorizationEndpoint { get; set; }
    public string? TokenEndpoint { get; set; }
    public string? UserInfoEndpoint { get; set; }
    public string? Scope { get; set; }
    public string? IconUrl { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public int DisplayOrder { get; set; }
} 