namespace DynamicERP.Core.Results;

public class PagedResult<T> : DataResult<List<T>>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalRecords { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    protected PagedResult(
        bool isSuccess,
        string message,
        List<T>? data,
        int pageNumber,
        int pageSize,
        int totalRecords,
        List<string>? errors = null)
        : base(isSuccess, message, data, errors)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    }

    public static PagedResult<T> Success(
        List<T> data,
        int pageNumber,
        int pageSize,
        int totalRecords,
        string message = "İşlem başarılı")
    {
        return new PagedResult<T>(true, message, data, pageNumber, pageSize, totalRecords);
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