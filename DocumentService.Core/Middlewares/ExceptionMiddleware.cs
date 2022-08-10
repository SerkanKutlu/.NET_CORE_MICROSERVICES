using System.Net;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Core.Middlewares;

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
            await HandleCustomExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
            
        }
    }
    
    private async Task HandleCustomExceptionAsync(HttpContext httpContext, CustomExceptionBase ex)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = ex.ErrorDetails.StatusCode;
        await httpContext.Response.WriteAsync(ex.ToString());
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsync(new ErrorDetails("Internal server error occured, try again",(int)HttpStatusCode.InternalServerError)
        .ToString());
    }
}