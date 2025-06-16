using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicERP.Core.Extensions;

/// <summary>
/// Loglama servisi için extension metodlar.
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Uygulama içerisinde oluşturulan Özel Loglama servisini DI container'a ekler.
    /// </summary>
    public static IServiceCollection AddLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerService, SerilogLoggerService>();
        return services;
    }

    /// <summary>
    /// Loglama servisini DI container'a ekler ve özel konfigürasyon sağlar.
    /// </summary>
    public static IServiceCollection AddLoggerService(this IServiceCollection services, Action<SerilogLoggerService> configure)
    {
        var loggerService = new SerilogLoggerService();
        configure(loggerService);
        services.AddSingleton<ILoggerService>(loggerService);
        return services;
    }
} 