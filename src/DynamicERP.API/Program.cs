using FluentValidation;
using DynamicERP.Application.Features.Users.Commands;
using DynamicERP.API;
using DynamicERP.Core.Extensions;
using DynamicERP.Core.Middleware;
using DynamicERP.Core.Models;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

builder.Services.AddDynamicErpServices(builder.Configuration);
builder.Services.AddLoggerService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DynamicERP API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Add CORS middleware
var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();
if (corsSettings?.AllowedOrigins?.Length > 0)
{
    app.UseCors("CorsPolicy");
}

app.UseMiddleware<RequestResponseLoggingMiddleware>();

// Add Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
