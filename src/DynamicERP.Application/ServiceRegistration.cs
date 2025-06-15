using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Domain.Interfaces;
using DynamicERP.Infrastructure.Data;
using DynamicERP.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        //services.AddScoped<IUserService, UserService>();
     

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