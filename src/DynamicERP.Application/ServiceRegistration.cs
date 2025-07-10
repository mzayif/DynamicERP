using DynamicERP.Application.Services;
using DynamicERP.Application.Common.Behaviors;
using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.Models;
using DynamicERP.Domain.Interfaces;
using DynamicERP.Infrastructure.Data;
using DynamicERP.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentValidation;
using System.Text;
using System.Text.Json;
using DynamicERP.Core.Services;
using Microsoft.Extensions.Options;

namespace DynamicERP.API;

public static class ServiceRegistration
{
    public static IServiceCollection AddDynamicErpServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
        
        // Add Validation Behavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Add JWT Settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

        // Add CORS Settings
        services.Configure<CorsSettings>(configuration.GetSection("CorsSettings"));
        var corsSettings = configuration.GetSection("CorsSettings").Get<CorsSettings>();

        // Add CORS
        if (corsSettings?.AllowedOrigins?.Length > 0)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins(corsSettings.AllowedOrigins)
                          .WithMethods(corsSettings.AllowedMethods)
                          .WithHeaders(corsSettings.AllowedHeaders)
                          .SetIsOriginAllowedToAllowWildcardSubdomains();
                    
                    if (corsSettings.AllowCredentials)
                        policy.AllowCredentials();
                    
                    policy.SetPreflightMaxAge(TimeSpan.FromSeconds(corsSettings.MaxAge));
                });
            });
        }

        // Add JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings?.SecretKey ?? "default-key")),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings?.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings?.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        // Add Authorization
        services.AddAuthorization();

        // Add DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            
            // Development ortamında detaylı hata mesajları
            if (configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // Add Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IExternalProviderRepository, ExternalProviderRepository>();
        
        // Add Dynamic Entity Repositories
        services.AddScoped<IEntitySchemaRepository, EntitySchemaRepository>();
        services.AddScoped<IFieldDefinitionRepository, FieldDefinitionRepository>();
        services.AddScoped<IDynamicEntityRepository, DynamicEntityRepository>();

        // Add Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ILoggerService, SerilogLoggerService>();

        // HTTP servis konfigürasyonları için Options Pattern
        services.Configure<HttpServiceOptions>(configuration.GetSection("HttpServiceConfigurations"));
        
        // HTTP servisi ekle
        services.AddHttpClient<IHttpService, HttpService>();
        services.AddScoped<IHttpService, HttpService>();

        // WCF servisi ekle
        services.AddHttpClient<IWcfService, WcfService>();
        services.AddScoped<IWcfService, WcfService>();
     
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

            // JWT Authentication için Swagger konfigürasyonu
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Mapster Configuration
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }


} 