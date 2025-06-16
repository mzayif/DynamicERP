using DynamicERP.Core.Constants;
using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.RequestModels;
using DynamicERP.Core.Results;
using FluentValidation;
using MediatR;

namespace DynamicERP.Application.Features.Users.Commands;

/// <summary>
/// Yeni kullanıcı oluşturmak için kullanılan requestCommand modeli
/// </summary>
public class CreateUserRequestCommand : CreateUserRequest, IRequest<Result>
{
}

/// <summary>
/// CreateUserRequestCommand için validator sınıfı
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestCommand>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Kullanıcı adı"))
            .MinimumLength(3).WithMessage(Messages.GetMessage(MessageCodes.Validation.MinLength, "Kullanıcı adı", 3))
            .MaximumLength(50).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Kullanıcı adı", 50));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Email"))
            .EmailAddress().WithMessage(Messages.GetMessage(MessageCodes.Validation.InvalidEmail))
            .MaximumLength(100).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Email", 100));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Şifre"))
            .MinimumLength(6).WithMessage(Messages.GetMessage(MessageCodes.Validation.MinLength, "Şifre", 6))
            .MaximumLength(50).WithMessage(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Şifre", 50));

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
/// CreateUserRequestCommand için handler sınıfı
/// </summary>
public class CreateUserHandler : IRequestHandler<CreateUserRequestCommand, Result>
{
    private readonly IUserService _userService;

    public CreateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(CreateUserRequestCommand requestCommand, CancellationToken cancellationToken)
    {
        var result = await _userService.CreateUserAsync(requestCommand, requestCommand.Password, cancellationToken);
        
        if (result.IsSuccess)
            return Result.Success(Messages.GetMessage(MessageCodes.Common.Success));
            
        return result;
    }
} 