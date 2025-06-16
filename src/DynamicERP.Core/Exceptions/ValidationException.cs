namespace DynamicERP.Core.Exceptions;

/// <summary>
/// Validasyon hataları için exception sınıfı.
/// </summary>
public class ValidationException : BaseException
{
    /// <summary>
    /// Validasyon hataları.
    /// </summary>
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(Dictionary<string, string[]> errors)
        : base("Validasyon hatası", "VALIDATION_ERROR")
    {
        Errors = errors;
    }
} 