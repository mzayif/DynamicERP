using DynamicERP.Core.Constants;

namespace DynamicERP.Core.Results;

public class PagedResult<T> : DataResult<T>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalRecords { get; }
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    protected PagedResult(
        bool isSuccess,
        string message,
        T? data,
        int pageNumber,
        int pageSize,
        int totalRecords,
        List<string>? errors = null)
        : base(isSuccess, message, data, errors)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
    }

    public static PagedResult<T> Success(
        T data,
        int pageNumber,
        int pageSize,
        int totalRecords,
        string message = null)
    {
        return new PagedResult<T>(true, message ?? Messages.GetMessage(MessageCodes.Common.Success), data, pageNumber, pageSize, totalRecords);
    }

    public static PagedResult<T> Failure(
        string message,
        int pageNumber,
        int pageSize,
        List<string>? errors = null)
    {
        return new PagedResult<T>(false, message, null, pageNumber, pageSize, 0, errors);
    }
} 