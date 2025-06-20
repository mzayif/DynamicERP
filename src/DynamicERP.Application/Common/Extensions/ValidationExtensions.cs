using FluentValidation;
using FluentValidation.Results;
using DynamicERP.Core.Results;
using DynamicERP.Core.Constants;

namespace DynamicERP.Application.Common.Extensions;

/// <summary>
/// Validation işlemleri için extension metodları
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// ValidationResult'ı Result'a dönüştürür
    /// </summary>
    /// <param name="validationResult">Validation sonucu</param>
    /// <returns>Result nesnesi</returns>
    public static Result ToResult(this ValidationResult validationResult)
    {
        if (validationResult.IsValid)
            return Result.Success();

        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        var errorMessage = string.Join("; ", errorMessages);
        
        return Result.Failure(errorMessage);
    }

    /// <summary>
    /// ValidationResult'ı DataResult'a dönüştürür
    /// </summary>
    /// <typeparam name="T">Data tipi</typeparam>
    /// <param name="validationResult">Validation sonucu</param>
    /// <param name="data">Data</param>
    /// <returns>DataResult nesnesi</returns>
    public static DataResult<T> ToDataResult<T>(this ValidationResult validationResult, T data)
    {
        if (validationResult.IsValid)
            return DataResult<T>.Success(data);

        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        var errorMessage = string.Join("; ", errorMessages);
        
        return DataResult<T>.Failure(errorMessage);
    }

    /// <summary>
    /// Validation kurallarını ValidationRules'dan alarak FluentValidation'a uygular
    /// </summary>
    /// <typeparam name="T">Model tipi</typeparam>
    /// <param name="ruleBuilder">Rule builder</param>
    /// <param name="propertyName">Property adı</param>
    /// <returns>Rule builder</returns>
    public static IRuleBuilderOptions<T, string> ApplyShortTextRules<T>(this IRuleBuilder<T, string> ruleBuilder, string propertyName)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, propertyName))
            .MinimumLength(ValidationRules.ShortText.MinLength).WithMessage(Messages.GetMessage(MessageCodes.Validation.MinLength, propertyName, ValidationRules.ShortText.MinLength))
            .MaximumLength(ValidationRules.ShortText.MaxLength).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, propertyName, ValidationRules.ShortText.MaxLength))
            .Matches(ValidationRules.ShortText.Pattern).WithMessage(Messages.GetMessage(MessageCodes.Validation.InvalidFormat, propertyName));
    }

    /// <summary>
    /// Email validation kurallarını uygular
    /// </summary>
    /// <typeparam name="T">Model tipi</typeparam>
    /// <param name="ruleBuilder">Rule builder</param>
    /// <param name="propertyName">Property adı</param>
    /// <returns>Rule builder</returns>
    public static IRuleBuilderOptions<T, string> ApplyEmailRules<T>(this IRuleBuilder<T, string> ruleBuilder, string propertyName)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, propertyName))
            .EmailAddress().WithMessage(Messages.GetMessage(MessageCodes.Validation.InvalidEmail))
            .MaximumLength(ValidationRules.Email.MaxLength).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, propertyName, ValidationRules.Email.MaxLength));
    }

    /// <summary>
    /// Şifre validation kurallarını uygular
    /// </summary>
    /// <typeparam name="T">Model tipi</typeparam>
    /// <param name="ruleBuilder">Rule builder</param>
    /// <param name="propertyName">Property adı</param>
    /// <returns>Rule builder</returns>
    public static IRuleBuilderOptions<T, string> ApplyPasswordRules<T>(this IRuleBuilder<T, string> ruleBuilder, string propertyName)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, propertyName))
            .MinimumLength(ValidationRules.Password.MinLength).WithMessage(Messages.GetMessage(MessageCodes.Validation.MinLength, propertyName, ValidationRules.Password.MinLength))
            .MaximumLength(ValidationRules.Password.MaxLength).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, propertyName, ValidationRules.Password.MaxLength))
            .Matches(ValidationRules.Password.Pattern).WithMessage(Messages.GetMessage(MessageCodes.Validation.InvalidPassword, propertyName));
    }

    /// <summary>
    /// Telefon numarası validation kurallarını uygular
    /// </summary>
    /// <typeparam name="T">Model tipi</typeparam>
    /// <param name="ruleBuilder">Rule builder</param>
    /// <param name="propertyName">Property adı</param>
    /// <returns>Rule builder</returns>
    public static IRuleBuilderOptions<T, string> ApplyPhoneRules<T>(this IRuleBuilder<T, string> ruleBuilder, string propertyName)
    {
        return ruleBuilder
            .MaximumLength(ValidationRules.Phone.MaxLength).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, propertyName, ValidationRules.Phone.MaxLength))
            .Matches(ValidationRules.Phone.Pattern).WithMessage(Messages.GetMessage(MessageCodes.Validation.InvalidPhone))
            .When(x => !string.IsNullOrEmpty(x.ToString()));
    }
} 