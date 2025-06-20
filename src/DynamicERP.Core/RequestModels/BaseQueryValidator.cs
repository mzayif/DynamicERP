using FluentValidation;
using DynamicERP.Core.Constants;

namespace DynamicERP.Core.RequestModels;

/// <summary>
/// BaseQuery için validator sınıfı
/// </summary>
public class BaseQueryValidator : AbstractValidator<BaseQuery>
{
    public BaseQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage(Messages.GetMessage(MessageCodes.Validation.GreaterThan, "Sayfa numarası", 0));

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage(Messages.GetMessage(MessageCodes.Validation.GreaterThan, "Sayfa boyutu", 0))
            .LessThanOrEqualTo(100).WithMessage(Messages.GetMessage(MessageCodes.Validation.LessThanOrEqualTo, "Sayfa boyutu", 100));

        RuleFor(x => x.SearchTerm)
            .MaximumLength(100).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Arama terimi", 100))
            .When(x => !string.IsNullOrEmpty(x.SearchTerm));

        RuleFor(x => x.SortBy)
            .MaximumLength(50).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Sıralama alanı", 50))
            .When(x => !string.IsNullOrEmpty(x.SortBy));
    }
} 