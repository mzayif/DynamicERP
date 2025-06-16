namespace DynamicERP.Core.Constants;

/// <summary>
/// Uygulama mesajları.
/// </summary>
public static class Messages
{
    private static readonly Dictionary<string, Dictionary<string, string>> _translations = new()
    {
        ["tr"] = new()
        {
            [MessageCodes.Common.NotFound] = "{0} bulunamadı.",
            [MessageCodes.Common.AlreadyExists] = "Bu {0} zaten mevcut.",
            [MessageCodes.Common.InvalidCredentials] = "Geçersiz kullanıcı adı veya şifre.",
            [MessageCodes.Common.AccountLocked] = "Hesabınız kilitlendi. Lütfen daha sonra tekrar deneyin.",
            [MessageCodes.Common.Error] = "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
            [MessageCodes.Common.Success] = "İşlem başarıyla tamamlandı.",
            [MessageCodes.Common.Updated] = "{0} başarıyla güncellendi.",
            [MessageCodes.Common.Unauthorized] = "Bu işlem için yetkiniz bulunmamaktadır.",
            [MessageCodes.Common.TokenExpired] = "Oturum süreniz dolmuştur. Lütfen tekrar giriş yapın.",
            [MessageCodes.Common.InvalidToken] = "Geçersiz token.",
            [MessageCodes.Validation.Required] = "{0} alanı zorunludur.",
            [MessageCodes.Validation.InvalidFormat] = "{0} alanı geçersiz formatta.",
            [MessageCodes.Validation.MinLength] = "{0} alanı en az {1} karakter olmalıdır.",
            [MessageCodes.Validation.MaxLength] = "{0} alanı en fazla {1} karakter olmalıdır.",
            [MessageCodes.Validation.InvalidValue] = "{0} alanı için geçersiz değer.",
            [MessageCodes.Validation.InvalidDate] = "{0} alanı için geçersiz tarih.",
            [MessageCodes.Validation.InvalidEmail] = "Geçersiz e-posta adresi.",
            [MessageCodes.Validation.InvalidPhone] = "Geçersiz telefon numarası."
        },
        ["en"] = new()
        {
            [MessageCodes.Common.NotFound] = "{0} not found.",
            [MessageCodes.Common.AlreadyExists] = "This {0} already exists.",
            [MessageCodes.Common.InvalidCredentials] = "Invalid username or password.",
            [MessageCodes.Common.AccountLocked] = "Your account has been locked. Please try again later.",
            [MessageCodes.Common.Error] = "An error occurred. Please try again later.",
            [MessageCodes.Common.Success] = "Operation completed successfully.",
            [MessageCodes.Common.Updated] = "{0} has been successfully updated.",
            [MessageCodes.Common.Unauthorized] = "You are not authorized to perform this action.",
            [MessageCodes.Common.TokenExpired] = "Your session has expired. Please login again.",
            [MessageCodes.Common.InvalidToken] = "Invalid token.",
            [MessageCodes.Validation.Required] = "{0} field is required.",
            [MessageCodes.Validation.InvalidFormat] = "{0} field has invalid format.",
            [MessageCodes.Validation.MinLength] = "{0} field must be at least {1} characters.",
            [MessageCodes.Validation.MaxLength] = "{0} field must be at most {1} characters.",
            [MessageCodes.Validation.InvalidValue] = "Invalid value for {0} field.",
            [MessageCodes.Validation.InvalidDate] = "Invalid date for {0} field.",
            [MessageCodes.Validation.InvalidEmail] = "Invalid email address.",
            [MessageCodes.Validation.InvalidPhone] = "Invalid phone number."
        }
    };

    /// <summary>
    /// Belirtilen kod ve dil için mesajı döndürür.
    /// </summary>
    public static string GetMessage(string code, params object[] parameters)
    {
        string language = "tr";
        // todo: Burada daha sonra login olan kullanıcının dil bilgisine göre düzenleme yapılabilir.
        var message = _translations[language][code];
        return parameters != null && parameters.Length > 0
            ? string.Format(message, parameters)
            : message;
    }
} 