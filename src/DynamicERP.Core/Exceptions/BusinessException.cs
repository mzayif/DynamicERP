namespace DynamicERP.Core.Exceptions;

/// <summary>
/// İş kuralı hataları için exception sınıfı.
/// </summary>
public class BusinessException : BaseException
{
    public BusinessException(string message, string errorCode = "BUSINESS_ERROR")
        : base(message, errorCode)
    {
    }
} 