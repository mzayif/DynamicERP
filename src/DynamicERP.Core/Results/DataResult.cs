using DynamicERP.Core.Constants;
using DynamicERP.Core.ResponseModels;

namespace DynamicERP.Core.Results;

public class DataResult<T> : Result
{
    public T? Data { get; }

    protected DataResult(bool isSuccess, string message, T? data, List<string>? errors = null)
        : base(isSuccess, message, errors)
    {
        Data = data;
    }

    public static DataResult<T> Success(T? data, string? message = null)
    {
        return new DataResult<T>(true, message ?? Messages.GetMessage(MessageCodes.Common.Success), data);
    }

    public static DataResult<T> Failure(string message, List<string>? errors = null)
    {
        return new DataResult<T>(false, message, default, errors);
    }
} 