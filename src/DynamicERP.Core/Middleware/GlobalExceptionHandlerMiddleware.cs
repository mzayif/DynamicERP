using DynamicERP.Core.Exceptions;
using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.Results;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using DynamicERP.Core.Constants;

namespace DynamicERP.Core.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerService _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILoggerService logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        Result result;
        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = Result.Failure(
                    Messages.GetMessage(MessageCodes.Validation.Required),
                    validationEx.Errors.SelectMany(e => e.Value).ToList());
                break;

            case BusinessException businessEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = Result.Failure(businessEx.Message);
                break;

            case NotFoundException notFoundEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                result = Result.Failure(Messages.GetMessage(MessageCodes.Common.NotFound, notFoundEx.EntityName));
                break;

            case UnauthorizedException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                result = Result.Failure(Messages.GetMessage(MessageCodes.Common.Unauthorized));
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = Result.Failure(Messages.GetMessage(MessageCodes.Common.Error));
                break;
        }

        _logger.LogError("Global exception handler caught an exception", exception);

        var jsonResult = JsonSerializer.Serialize(result);
        await response.WriteAsync(jsonResult);
    }
} 