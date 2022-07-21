using CustomerService.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CustomerService.Common.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (CustomExceptionBase ex)
        {
            //Logging
            await HandleCustomExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            //Logging
            await HandleCustomExceptionAsync(httpContext, ex);
        }
    }
    
    public async Task HandleCustomExceptionAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 500;
        if (ex is CustomExceptionBase)
        {
            httpContext.Response.StatusCode = ((CustomExceptionBase) ex).ErrorDetails.StatusCode;
        }
        
        await httpContext.Response.WriteAsync(ex.ToString());
    }
}