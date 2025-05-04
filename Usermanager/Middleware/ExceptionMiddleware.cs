namespace Usermanager.Middleware;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Usermanager.Model;

public class ExceptionMiddleware(ILogger<ExceptionMiddleware> _logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
           await next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An unexpected error occurred.";

        switch (exception)
        {
            case NotFoundException notFoundEx:
                statusCode = HttpStatusCode.NotFound;
                message = notFoundEx.Message;
                break;
            case UnauthorizedAccessException unauthorizedEx:
                statusCode = HttpStatusCode.Unauthorized;
                message = unauthorizedEx.Message;
                break;
            case ArgumentException argEx:
                statusCode = HttpStatusCode.BadRequest;
                message = argEx.Message;
                break;
            default:
                message = exception.Message;
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        var result = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(result);
    }
}
