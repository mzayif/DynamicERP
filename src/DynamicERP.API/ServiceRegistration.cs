using DynamicERP.Core.Interfaces.Repositories;
using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Infrastructure;
using DynamicERP.Infrastructure.Data;
using DynamicERP.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace DynamicERP.API;

public static class ServiceRegistration
{
    public static IServiceCollection AddDynamicErpServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Add Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IExternalProviderRepository, ExternalProviderRepository>();

        // Add Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IExternalProviderService, ExternalProviderService>();

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DynamicERP API",
                Version = "v1",
                Description = "DynamicERP API Documentation",
                Contact = new OpenApiContact
                {
                    Name = "DynamicERP Team",
                    Email = "info@dynamicerp.com"
                }
            });
        });

        return services;
    }
} 