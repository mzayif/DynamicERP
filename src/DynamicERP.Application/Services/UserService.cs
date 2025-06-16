using DynamicERP.Application.Features.Users.Commands;
using DynamicERP.Core.Constants;
using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.RequestModels;
using DynamicERP.Core.ResponseModels;
using DynamicERP.Core.Results;
using DynamicERP.Domain.Entities;
using DynamicERP.Domain.Interfaces;
using Mapster;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DynamicERP.Application.Services;

/// <summary>
/// Kullanıcı işlemleri için servis implementasyonu
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<DataResult<UserResponseModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, true, cancellationToken);
        if (user == null)
            return DataResult<UserResponseModel>.Failure(Messages.GetMessage(MessageCodes.Common.NotFound, "Kullanıcı"));

        var response = user.Adapt<UserResponseModel>();
        return DataResult<UserResponseModel>.Success(response);
    }

    public async Task<DataResult<UserResponseModel>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, true, cancellationToken);
        if (user == null)
            return DataResult<UserResponseModel>.Failure(Messages.GetMessage(MessageCodes.Common.NotFound, "Kullanıcı"));

        var response = user.Adapt<UserResponseModel>();
        return DataResult<UserResponseModel>.Success(response);
    }

    public async Task<DataResult<UserResponseModel>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
            return DataResult<UserResponseModel>.Failure(Messages.GetMessage(MessageCodes.Common.NotFound, "Kullanıcı"));

        var response = user.Adapt<UserResponseModel>();
        return DataResult<UserResponseModel>.Success(response);
    }

    public async Task<DataResult<IEnumerable<UserResponseModel>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAll().ToListAsync(cancellationToken);
        var response = users.Adapt<IEnumerable<UserResponseModel>>();
        return DataResult<IEnumerable<UserResponseModel>>.Success(response);
    }

    public async Task<Result> CreateUserAsync(CreateUserRequest request, string? password, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.AlreadyExists, "Kullanıcı"));

        if (await _userRepository.ExistsByUsernameAsync(request.Username))
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.AlreadyExists, "Kullanıcı"));

        var user = request.Adapt<User>();
        // TODO: Password hash işlemi eklenecek
        user.PasswordHash = password;

        await _userRepository.AddAsync(user, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> CreateExternalUserAsync(CreateUserRequest request, string externalId, string provider, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.AlreadyExists, "email adresi"));

        if (await _userRepository.ExistsByUsernameAsync(request.Username))
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.AlreadyExists, "kullanıcı adı"));

        var user = new User
        {
            Email = request.Email,
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            ProfilePictureUrl = request.ProfilePictureUrl,
            ExternalId = externalId,
            ExternalProvider = provider
        };

        await _userRepository.AddAsync(user, cancellationToken);
        return Result.Success(Messages.GetMessage(MessageCodes.Common.Success));
    }

    public async Task<Result> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, true, cancellationToken);
        if (user == null)
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.NotFound, "Kullanıcı"));

        if (request.Email != user.Email && await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.AlreadyExists, "Kullanıcı"));

        if (request.Username != user.Username && await _userRepository.ExistsByUsernameAsync(request.Username))
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.AlreadyExists, "Kullanıcı"));

        request.Adapt(user);
        await _userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await _userRepository.ExistsAsync(id, cancellationToken))
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.NotFound, "Kullanıcı"));

        await _userRepository.DeleteAsync(id, cancellationToken);
        return Result.Success(Messages.GetMessage(MessageCodes.Common.Success));
    }

    public async Task<Result> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, false, cancellationToken);
        if (user == null)
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.NotFound, "Kullanıcı"));

        // TODO: Password doğrulama işlemi eklenecek

        return Result.Success(Messages.GetMessage(MessageCodes.Common.Success));
    }

    public async Task<Result> ValidateExternalCredentialsAsync(string externalId, string provider, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByExternalIdAsync(externalId, provider);
        if (user == null)
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.NotFound, "Kullanıcı"));

        return Result.Success(Messages.GetMessage(MessageCodes.Common.Success));
    }

    public async Task<Result> UpdateLastLoginAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, true, cancellationToken);
        if (user == null)
            return Result.Failure(Messages.GetMessage(MessageCodes.Common.NotFound, "Kullanıcı"));

        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success(Messages.GetMessage(MessageCodes.Common.Success));
    }
}