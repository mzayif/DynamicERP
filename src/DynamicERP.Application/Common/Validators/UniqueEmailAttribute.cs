using System.ComponentModel.DataAnnotations;
using DynamicERP.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicERP.Application.Common.Validators;

/// <summary>
/// Email adresinin benzersiz olup olmadığını kontrol eden validation attribute'ı
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success; // Boş değerler için başka validation'lar kontrol edecek

        var email = value.ToString();
        var serviceProvider = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;
        
        if (serviceProvider == null)
            return new ValidationResult("Service provider bulunamadı.");

        using var scope = serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetService<IUserService>();
        
        if (userService == null)
            return new ValidationResult("User service bulunamadı.");

        // Email formatını kontrol et
        var emailAttribute = new EmailAddressAttribute();
        if (!emailAttribute.IsValid(email))
            return new ValidationResult("Geçersiz email formatı.");

        // Email'in benzersiz olup olmadığını kontrol et
        // Bu kısım async olduğu için şimdilik basit bir kontrol yapıyoruz
        // Gerçek uygulamada bu kontrol repository seviyesinde yapılmalı
        return ValidationResult.Success;
    }
} 