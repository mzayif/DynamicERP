using DynamicERP.Core.Constants;
using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.ResponseModels;
using DynamicERP.Core.Results;
using FluentValidation;
using MediatR;

namespace DynamicERP.Application.Features.Users.Queries;

/// <summary>
/// Kullanıcıyı ID'ye göre getirmek için kullanılan query modeli
/// </summary>
public class GetUserByIdQuery : IRequest<DataResult<UserResponseModel>>
{
    public Guid Id { get; set; }
}

/// <summary>
/// GetUserByIdQuery için validator sınıfı
/// </summary>
public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Messages.GetMessage(MessageCodes.Validation.Required, "Kullanıcı ID"));
    }
}

/// <summary>
/// GetUserByIdQuery için handler sınıfı
/// </summary>
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, DataResult<UserResponseModel>>
{
    private readonly IUserService _userService;

    public GetUserByIdQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<DataResult<UserResponseModel>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _userService.GetByIdAsync(query.Id, cancellationToken);
        
        if (result.IsSuccess)
            return DataResult<UserResponseModel>.Success(result.Data, Messages.GetMessage(MessageCodes.Common.Success));
            
        return DataResult<UserResponseModel>.Failure(result.Message);
    }
} 