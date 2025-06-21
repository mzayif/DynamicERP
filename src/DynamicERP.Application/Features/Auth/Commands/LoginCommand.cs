using DynamicERP.Core.Constants;
using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.RequestModels;
using DynamicERP.Core.ResponseModels;
using DynamicERP.Core.Results;
using FluentValidation;
using MediatR;

namespace DynamicERP.Application.Features.Auth.Commands;

/// <summary>
/// Kullanıcı girişi için command modeli
/// </summary>
public class LoginCommand : LoginRequest, IRequest<DataResult<LoginResponse>>
{
}

/// <summary>
/// LoginCommand için validator sınıfı
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Email"))
            .EmailAddress().WithMessage(Messages.GetMessage(MessageCodes.Validation.InvalidEmail));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Şifre"))
            .MinimumLength(6).WithMessage(Messages.GetMessage(MessageCodes.Validation.MinLength, "Şifre", 6));
    }
}

/// <summary>
/// LoginCommand için handler sınıfı
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, DataResult<LoginResponse>>
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    public async Task<DataResult<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        // Kullanıcı kimlik bilgilerini doğrula
        var validationResult = await _userService.ValidateCredentialsAsync(command.Email, command.Password, cancellationToken);
        
        if (!validationResult.IsSuccess)
            return DataResult<LoginResponse>.Failure(validationResult.Message);

        // Kullanıcı bilgilerini getir
        var userResult = await _userService.GetByEmailAsync(command.Email, cancellationToken);
        
        if (!userResult.IsSuccess)
            return DataResult<LoginResponse>.Failure(userResult.Message);

        // JWT token oluştur
        var accessToken = _jwtService.GenerateToken(userResult.Data);

        // Refresh token oluştur (RememberMe true ise)
        string? refreshToken = null;
        if (command.RememberMe)
        {
            refreshToken = _jwtService.GenerateRefreshToken(userResult.Data.Id);
        }

        // Son giriş tarihini güncelle
        await _userService.UpdateLastLoginAsync(userResult.Data.Id, cancellationToken);

        // Login response oluştur
        var loginResponse = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresInMinutes = 60, // JWT ayarlarından alınabilir
            User = userResult.Data
        };

        return DataResult<LoginResponse>.Success(loginResponse, Messages.GetMessage(MessageCodes.Common.Success));
    }
} 