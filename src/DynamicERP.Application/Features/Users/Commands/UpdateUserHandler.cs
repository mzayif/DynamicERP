using DynamicERP.Core.Constants;
using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.RequestModels;
using DynamicERP.Core.Results;
using FluentValidation;
using MediatR;

namespace DynamicERP.Application.Features.Users.Commands;

/// <summary>
/// Kullanıcı güncellemek için kullanılan requestCommand modeli
/// </summary>
public class UpdateUserRequestCommand :UpdateUserRequest, IRequest<Result>
{
}

/// <summary>
/// UpdateUserRequestCommand için validator sınıfı
/// </summary>
public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestCommand>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Kullanıcı ID"));

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Kullanıcı adı"))
            .MinimumLength(3).WithMessage(Messages.GetMessage(MessageCodes.Validation.MinLength, "Kullanıcı adı", 3))
            .MaximumLength(50).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Kullanıcı adı", 50));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Email"))
            .EmailAddress().WithMessage(Messages.GetMessage(MessageCodes.Validation.InvalidEmail))
            .MaximumLength(100).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Email", 100));

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Ad"))
            .MaximumLength(50).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Ad", 50));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Soyad"))
            .MaximumLength(50).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Soyad", 50));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Telefon numarası", 20))
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.ProfilePictureUrl)
            .MaximumLength(500).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Profil resmi URL'i", 500))
            .When(x => !string.IsNullOrEmpty(x.ProfilePictureUrl));
    }
}

/// <summary>
/// UpdateUserRequestCommand için handler sınıfı
/// </summary>
public class UpdateUserHandler : IRequestHandler<UpdateUserRequestCommand, Result>
{
    private readonly IUserService _userService;

    public UpdateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(UpdateUserRequestCommand requestCommand, CancellationToken cancellationToken)
    {
        return await _userService.UpdateUserAsync(requestCommand, cancellationToken);
    }
}