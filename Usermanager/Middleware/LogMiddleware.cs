using Microsoft.AspNetCore.Mvc.Controllers;

namespace Usermanager.Middleware;

public class RequestLoggingMiddleware : IMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();
        var controllerName = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerName ?? "Unknown";
        var actionName = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ActionName ?? "Unknown";

        var originalBodyStream = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        string exceptionMessage = "None";

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            exceptionMessage = ex.ToString();
            context.Response.StatusCode = 500;
            _logger.LogError(ex, "Unhandled exception");
        }

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var statusCode = context.Response.StatusCode;

        _logger.LogInformation(
            "Controller: {Controller}, Action: {Action}, Result: {StatusCode}, Exception: {Exception}",
            controllerName,
            actionName,
            statusCode,
            exceptionMessage
        );

        await responseBody.CopyToAsync(originalBodyStream);
    }
}

