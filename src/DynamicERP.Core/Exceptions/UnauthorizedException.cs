namespace DynamicERP.Core.Exceptions;

/// <summary>
/// Yetkisiz erişim hataları için exception sınıfı.
/// </summary>
public class UnauthorizedException : BaseException
{
    public UnauthorizedException()
        : base("Bu işlem için yetkiniz bulunmamaktadır", "UNAUTHORIZED")
    {
    }
} 