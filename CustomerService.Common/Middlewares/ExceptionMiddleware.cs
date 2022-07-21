using System.Net;
using CustomerService.Common.Exceptions;
using CustomerService.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CustomerService.Common.Middlewares;

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
        catch (MongoDuplicateKeyException ex)
        {
            await HandleCustomExceptionAsync(httpContext, new DuplicatedEmailException());
        }
        catch (MongoWriteException ex)
        {
            if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                await HandleCustomExceptionAsync(httpContext, new DuplicatedEmailException());
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
            
        }
    }
    
    private async Task HandleCustomExceptionAsync(HttpContext httpContext, CustomExceptionBase ex)
    {
        _logger.LogInformation($"Custom exception handled: {ex.ErrorDetails.Message}");
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = ((CustomExceptionBase) ex).ErrorDetails.StatusCode;
        await httpContext.Response.WriteAsync(ex.ToString());
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        _logger.LogError($"ERROR OCCURED: {ex.Message}");
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsync(new ErrorDetails("Internal server error occured, try again",(int)HttpStatusCode.InternalServerError)
        .ToString());
    }
}