using DynamicERP.Core.Constants;
using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.RequestModels;
using DynamicERP.Core.ResponseModels;
using DynamicERP.Core.Results;
using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace DynamicERP.Application.Features.Users.Queries;

/// <summary>
/// Tüm kullanıcıları getirmek için kullanılan query modeli
/// </summary>
public class GetAllUsersQuery : BaseQuery, IRequest<DataResult<List<UserResponseModel>>>
{
    // BaseQuery'den gelen özellikler:
    // - Page
    // - PageSize
    // - SearchTerm
    // - SortBy
    // - IsAscending
}

/// <summary>
/// GetAllUsersQuery için validator sınıfı
/// </summary>
public class GetAllUsersQueryValidator : BaseQueryValidator
{
    public GetAllUsersQueryValidator()
    {
        // BaseQueryValidator'dan gelen kurallar otomatik olarak uygulanır
        // Burada GetAllUsersQuery'ye özel kurallar eklenebilir
    }
}

/// <summary>
/// GetAllUsersQuery için handler sınıfı
/// </summary>
public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, DataResult<List<UserResponseModel>>>
{
    private readonly IUserService _userService;

    public GetAllUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<DataResult<List<UserResponseModel>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var result = await _userService.GetPageAbleAllUsersAsync(query, cancellationToken);

        if (result.IsSuccess)
            return DataResult<List<UserResponseModel>>.Success((List<UserResponseModel>?)result.Data, Messages.GetMessage(MessageCodes.Common.Success));

        return DataResult<List<UserResponseModel>>.Failure(result.Message);
    }
}