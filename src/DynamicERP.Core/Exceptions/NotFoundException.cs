namespace DynamicERP.Core.Exceptions;

/// <summary>
/// Kayıt bulunamadı hataları için exception sınıfı.
/// </summary>
public class NotFoundException : BaseException
{
    /// <summary>
    /// Bulunamayan kaydın adı.
    /// </summary>
    public string EntityName { get; }

    public NotFoundException(string entityName)
        : base($"{entityName} bulunamadı", "NOT_FOUND")
    {
        EntityName = entityName;
    }
} 