using System.ComponentModel.DataAnnotations;
using DynamicERP.Core.Constants;

namespace DynamicERP.Application.Common.Validators;

/// <summary>
/// Şifrenin güvenlik kurallarına uygun olup olmadığını kontrol eden validation attribute'ı
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ValidPasswordAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success; // Boş değerler için başka validation'lar kontrol edecek

        var password = value.ToString();

        // Minimum uzunluk kontrolü
        if (password.Length < ValidationRules.Password.MinLength)
            return new ValidationResult($"Şifre en az {ValidationRules.Password.MinLength} karakter olmalıdır.");

        // Maksimum uzunluk kontrolü
        if (password.Length > ValidationRules.Password.MaxLength)
            return new ValidationResult($"Şifre en fazla {ValidationRules.Password.MaxLength} karakter olmalıdır.");

        // Şifre formatı kontrolü (en az 1 büyük harf, 1 küçük harf, 1 rakam ve 1 özel karakter)
        if (!System.Text.RegularExpressions.Regex.IsMatch(password, ValidationRules.Password.Pattern))
            return new ValidationResult("Şifre en az 1 büyük harf, 1 küçük harf, 1 rakam ve 1 özel karakter içermelidir.");

        return ValidationResult.Success;
    }
} 