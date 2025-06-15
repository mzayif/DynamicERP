namespace DynamicERP.Core.Constants;

/// <summary>
/// Entity'ler için validasyon kurallarını içeren sabit sınıfı.
/// Bu kurallar hem Entity Framework konfigürasyonları hem de FluentValidation için kullanılacaktır.
/// </summary>
public static class ValidationRules
{
    /// <summary>
    /// Kısa metin alanları için kurallar (Kullanıcı adı, kod, telefon vb.)
    /// </summary>
    public static class ShortText
    {
        /// <summary>
        /// Maksimum uzunluk: 50 karakter
        /// </summary>
        public const int MaxLength = 50;

        /// <summary>
        /// Minimum uzunluk: 2 karakter
        /// </summary>
        public const int MinLength = 2;

        /// <summary>
        /// Regex pattern: Sadece harf, rakam ve alt çizgi
        /// </summary>
        public const string Pattern = "^[a-zA-Z0-9_]+$";
    }

    /// <summary>
    /// Orta uzunluktaki metin alanları için kurallar (Açıklama, adres vb.)
    /// </summary>
    public static class MediumText
    {
        /// <summary>
        /// Maksimum uzunluk: 200 karakter
        /// </summary>
        public const int MaxLength = 200;

        /// <summary>
        /// Minimum uzunluk: 3 karakter
        /// </summary>
        public const int MinLength = 3;
    }

    /// <summary>
    /// Uzun metin alanları için kurallar (İçerik, detaylı açıklama vb.)
    /// </summary>
    public static class LongText
    {
        /// <summary>
        /// Maksimum uzunluk: 2000 karakter
        /// </summary>
        public const int MaxLength = 2000;

        /// <summary>
        /// Minimum uzunluk: 10 karakter
        /// </summary>
        public const int MinLength = 10;
    }

    /// <summary>
    /// E-posta alanları için kurallar
    /// </summary>
    public static class Email
    {
        /// <summary>
        /// Maksimum uzunluk: 100 karakter
        /// </summary>
        public const int MaxLength = 100;

        /// <summary>
        /// Regex pattern: Standart e-posta formatı
        /// </summary>
        public const string Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    }

    /// <summary>
    /// Telefon numarası alanları için kurallar
    /// </summary>
    public static class Phone
    {
        /// <summary>
        /// Maksimum uzunluk: 20 karakter
        /// </summary>
        public const int MaxLength = 20;

        /// <summary>
        /// Regex pattern: Uluslararası telefon formatı
        /// </summary>
        public const string Pattern = @"^\+?[0-9]{1,4}?[-. ]?\(?[0-9]{1,3}?\)?[-. ]?[0-9]{1,4}[-. ]?[0-9]{1,4}[-. ]?[0-9]{1,9}$";
    }

    /// <summary>
    /// URL alanları için kurallar
    /// </summary>
    public static class Url
    {
        /// <summary>
        /// Maksimum uzunluk: 200 karakter
        /// </summary>
        public const int MaxLength = 200;

        /// <summary>
        /// Regex pattern: Standart URL formatı
        /// </summary>
        public const string Pattern = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";
    }

    /// <summary>
    /// Şifre alanları için kurallar
    /// </summary>
    public static class Password
    {
        /// <summary>
        /// Maksimum uzunluk: 100 karakter
        /// </summary>
        public const int MaxLength = 100;

        /// <summary>
        /// Minimum uzunluk: 8 karakter
        /// </summary>
        public const int MinLength = 8;

        /// <summary>
        /// Regex pattern: En az 1 büyük harf, 1 küçük harf, 1 rakam ve 1 özel karakter
        /// </summary>
        public const string Pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
    }

    /// <summary>
    /// Para birimi alanları için kurallar
    /// </summary>
    public static class Currency
    {
        /// <summary>
        /// Maksimum uzunluk: 3 karakter (ISO 4217)
        /// </summary>
        public const int MaxLength = 3;

        /// <summary>
        /// Regex pattern: Sadece büyük harf
        /// </summary>
        public const string Pattern = "^[A-Z]{3}$";
    }

    /// <summary>
    /// Dil kodu alanları için kurallar
    /// </summary>
    public static class Language
    {
        /// <summary>
        /// Maksimum uzunluk: 2 karakter (ISO 639-1)
        /// </summary>
        public const int MaxLength = 2;

        /// <summary>
        /// Regex pattern: Sadece küçük harf
        /// </summary>
        public const string Pattern = "^[a-z]{2}$";
    }

    /// <summary>
    /// Zaman dilimi alanları için kurallar
    /// </summary>
    public static class TimeZone
    {
        /// <summary>
        /// Maksimum uzunluk: 50 karakter
        /// </summary>
        public const int MaxLength = 50;

        /// <summary>
        /// Regex pattern: IANA zaman dilimi formatı
        /// </summary>
        public const string Pattern = @"^[A-Za-z]+/[A-Za-z_]+$";
    }
} 