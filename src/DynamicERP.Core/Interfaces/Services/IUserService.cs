using DynamicERP.Core.RequestModels;
using DynamicERP.Core.ResponseModels;
using DynamicERP.Core.Results;

namespace DynamicERP.Core.Interfaces.Services;

public interface IUserService
{
    Task<DataResult<UserResponseModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DataResult<UserResponseModel>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<DataResult<UserResponseModel>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<DataResult<IEnumerable<UserResponseModel>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DataResult<IEnumerable<UserResponseModel>>> GetPageAbleAllUsersAsync(BaseQuery query, CancellationToken cancellationToken);

    Task<Result> CreateUserAsync(CreateUserRequest user, string? password = null, CancellationToken cancellationToken = default);
    Task<Result> CreateExternalUserAsync(CreateUserRequest user, string externalId, string provider, CancellationToken cancellationToken = default);
    Task<Result> UpdateUserAsync(UpdateUserRequest user, CancellationToken cancellationToken = default);
    Task<Result> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result> ValidateExternalCredentialsAsync(string externalId, string provider, CancellationToken cancellationToken = default);
    Task<Result> UpdateLastLoginAsync(Guid userId, CancellationToken cancellationToken = default);
} 