using System.Security.Claims;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Core.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _factory;
    public AuthMiddleware(RequestDelegate next, IHttpClientFactory factory)
    {
        _next = next;
        _factory = factory;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var userClaims = ((ClaimsIdentity)httpContext.User.Identity)?.Claims.ToList();
        var userId  = userClaims?.FirstOrDefault(c => c.Type == "UserId")?.Value;
        var response = _factory.CreateClient().GetAsync($"https://localhost:7181/api/User/validate/token/{userId}").Result;
        if(response.IsSuccessStatusCode)
            await _next(httpContext);
        else
        {
            throw new InvalidTokenException();
        }
       
    }
}