using FluentValidation;
using MediatR;
using DynamicERP.Core.Results;
using DynamicERP.Core.Constants;

namespace DynamicERP.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline'ında validation'ı otomatik olarak çalıştıran behavior sınıfı
/// </summary>
/// <typeparam name="TRequest">Request tipi</typeparam>
/// <typeparam name="TResponse">Response tipi</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var errorMessages = failures.Select(f => f.ErrorMessage).ToList();
            var errorMessage = string.Join("; ", errorMessages);

            // Eğer TResponse Result tipindeyse
            if (typeof(TResponse) == typeof(Result))
            {
                var result = Result.Failure(errorMessage);
                return (TResponse)(object)result;
            }

            // Eğer TResponse DataResult<T> tipindeyse
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(DataResult<>))
            {
                var resultType = typeof(DataResult<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]);
                var failureMethod = resultType.GetMethod("Failure", new[] { typeof(string) });
                var result = failureMethod.Invoke(null, new object[] { errorMessage });
                return (TResponse)result;
            }

            // Diğer durumlar için exception fırlat
            throw new ValidationException(failures);
        }

        return await next();
    }
} 