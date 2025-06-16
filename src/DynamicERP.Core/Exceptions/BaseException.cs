namespace DynamicERP.Core.Exceptions;

/// <summary>
/// Uygulama için temel exception sınıfı.
/// </summary>
public abstract class BaseException : Exception
{
    /// <summary>
    /// Hata kodu.
    /// </summary>
    public string ErrorCode { get; }

    protected BaseException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
} 