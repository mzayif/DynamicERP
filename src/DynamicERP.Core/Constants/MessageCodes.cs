namespace DynamicERP.Core.Constants;

/// <summary>
/// Uygulama mesaj kodları. <br/>
/// <b>Önemli :</b> Oluşturulan yeni mesajların kullanımı için örnek metin türkçe olarak prop üstüne eklenmelidir.
/// </summary>
public static class MessageCodes
{
    public static class Common
    {
        /// <summary>
        /// {0} bulunamadı.
        /// </summary>
        public const string NotFound = "RECORD_NOT_FOUND";
        /// <summary>
        /// Bu {0} zaten mevcut.
        /// </summary>
        public const string AlreadyExists = "RECORD_ALREADY_EXISTS";
        /// <summary>
        /// Geçersiz kullanıcı adı veya şifre.
        /// </summary>
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        /// <summary>
        /// Hesabınız kilitlendi. Lütfen daha sonra tekrar deneyin.
        /// </summary>
        public const string AccountLocked = "ACCOUNT_LOCKED";
        /// <summary>
        /// Bir hata oluştu. Lütfen daha sonra tekrar deneyin.
        /// </summary>
        public const string Error = "ERROR";
        /// <summary>
        /// İşlem başarıyla tamamlandı.
        /// </summary>
        public const string Success = "SUCCESS";
        /// <summary>
        /// Bu işlem için yetkiniz bulunmamaktadır.
        /// </summary>
        public const string Unauthorized = "UNAUTHORIZED";
        /// <summary>
        /// Oturum süreniz dolmuştur. Lütfen tekrar giriş yapın.
        /// </summary>
        public const string TokenExpired = "TOKEN_EXPIRED";
        /// <summary>
        /// Geçersiz token.
        /// </summary>
        public const string InvalidToken = "INVALID_TOKEN";
    }

    public static class Validation
    {
        /// <summary>
        /// {0} alanı zorunludur.
        /// </summary>
        public const string Required = "VALIDATION_REQUIRED";
        /// <summary>
        /// {0} alanı geçersiz formatta.
        /// </summary>
        public const string InvalidFormat = "VALIDATION_INVALID_FORMAT";
        /// <summary>
        /// {0} alanı en az {1} karakter olmalıdır.
        /// </summary>
        public const string MinLength = "VALIDATION_MIN_LENGTH";
        /// <summary>
        /// {0} alanı en fazla {1} karakter olmalıdır.
        /// </summary>
        public const string MaxLength = "VALIDATION_MAX_LENGTH";
        /// <summary>
        /// {0} alanı için geçersiz değer.
        /// </summary>
        public const string InvalidValue = "VALIDATION_INVALID_VALUE";
        /// <summary>
        /// {0} alanı için geçersiz tarih.
        /// </summary>
        public const string InvalidDate = "VALIDATION_INVALID_DATE";
        /// <summary>
        /// Geçersiz e-posta adresi.
        /// </summary>
        public const string InvalidEmail = "VALIDATION_INVALID_EMAIL";
        /// <summary>
        /// Geçersiz telefon numarası.
        /// </summary>
        public const string InvalidPhone = "VALIDATION_INVALID_PHONE";
    }
} 