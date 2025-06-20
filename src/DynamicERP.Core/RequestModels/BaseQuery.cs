using DynamicERP.Core.Constants;

namespace DynamicERP.Core.RequestModels;

/// <summary>
/// Tüm liste sorguları için temel query modeli
/// </summary>
public abstract class BaseQuery
{
    /// <summary>
    /// Sayfa numarası (varsayılan: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Sayfa boyutu (varsayılan: 10)
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Arama terimi
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Sıralama alanı
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Artan sıralama (true: artan, false: azalan)
    /// </summary>
    public bool IsAscending { get; set; } = true;
}
