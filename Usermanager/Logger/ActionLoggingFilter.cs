

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Usermanager.Logger;
public class LogActionFilter : IActionFilter, IExceptionFilter
{
    private readonly ILogger<LogActionFilter> _logger;

    public LogActionFilter(ILogger<LogActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // No log here — we only log once the action is done
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        var controller = context.RouteData.Values["controller"]?.ToString() ?? "Unknown";
        var action = context.RouteData.Values["action"]?.ToString() ?? "Unknown";
        var result = context.Exception == null ? "Success" : "Failed";
        var exceptionMessage = context.Exception?.Message ?? "None";

        _logger.LogInformation("Controller: {Controller} - Action: {Action} - Result: {Result} - Exception: {Exception}",
            controller, action, result, exceptionMessage);
    }

    public void OnException(ExceptionContext context)
    {
        // Optional if you want separate log for exceptions
    }
}



