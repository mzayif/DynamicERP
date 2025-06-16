using DynamicERP.Core.Constants;

namespace DynamicERP.Core.Results;

public class Result
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public List<string>? Errors { get; }

    protected Result(bool isSuccess, string message, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors;
    }

    public static Result Success(string? message = null)
    {
        return new Result(true, message ?? Messages.GetMessage(MessageCodes.Common.Success));
    }

    public static Result Failure(string message, List<string>? errors = null)
    {
        return new Result(false, message, errors);
    }
} 