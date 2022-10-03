using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Http;
using UserService.Core.Exceptions;
namespace UserService.Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (CustomExceptionBase ex)
        {
            await HandleCustomExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
            
        }
    }
    
    private async Task HandleCustomExceptionAsync(Microsoft.AspNetCore.Http.HttpContext httpContext, CustomExceptionBase ex)
    {
        _logger.LogInformation($"Custom exception handled: {ex.ErrorDetails.Message}");
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = ex.ErrorDetails.StatusCode;
        await httpContext.Response.WriteAsync(ex.ToString());
    }

    private async Task HandleExceptionAsync(Microsoft.AspNetCore.Http.HttpContext httpContext, Exception ex)
    {
        _logger.LogError($"ERROR OCCURED: {ex.Message}");
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsync(new ErrorDetails("Internal server error occured, try again",(int)HttpStatusCode.InternalServerError)
        .ToString());
    }
}    