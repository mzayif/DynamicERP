namespace DynamicERP.Core.Constants;

public static class DefaultCodes
{
    public static Guid TestTenant { get; set; } = Guid.Parse("11111111-1111-1111-1111-111111111111");
    /// <summary>
    /// Refresh token'ın geçerlilik süresi (gün cinsinden)
    /// </summary>
    public static int RefreshTokenExpireDay { get; set; } = 7;
}