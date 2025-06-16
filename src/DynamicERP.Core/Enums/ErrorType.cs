namespace DynamicERP.Core.Enums;

/// <summary>
/// Hata tipleri.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Validasyon hatası.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// İş kuralı hatası.
    /// </summary>
    Business = 2,

    /// <summary>
    /// Kayıt bulunamadı hatası.
    /// </summary>
    NotFound = 3,

    /// <summary>
    /// Yetkisiz erişim hatası.
    /// </summary>
    Unauthorized = 4,

    /// <summary>
    /// Sistem hatası.
    /// </summary>
    System = 5
} 