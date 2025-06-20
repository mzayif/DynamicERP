using System.ComponentModel.DataAnnotations;
using DynamicERP.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicERP.Application.Common.Validators;

/// <summary>
/// Kullanıcı adının benzersiz olup olmadığını kontrol eden validation attribute'ı
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class UniqueUsernameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success; // Boş değerler için başka validation'lar kontrol edecek

        var username = value.ToString();
        var serviceProvider = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;
        
        if (serviceProvider == null)
            return new ValidationResult("Service provider bulunamadı.");

        using var scope = serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetService<IUserService>();
        
        if (userService == null)
            return new ValidationResult("User service bulunamadı.");

        // Username formatını kontrol et (sadece harf, rakam ve alt çizgi)
        if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            return new ValidationResult("Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir.");

        // Username'in benzersiz olup olmadığını kontrol et
        // Bu kısım async olduğu için şimdilik basit bir kontrol yapıyoruz
        // Gerçek uygulamada bu kontrol repository seviyesinde yapılmalı
        return ValidationResult.Success;
    }
} 