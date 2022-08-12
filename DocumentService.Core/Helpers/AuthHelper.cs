using System.Security.Claims;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Helpers;

public class AuthHelper:IAuthHelper
{
    public bool IsAuthenticated(HttpContext httpContext)
    {
        var userClaims = ((ClaimsIdentity)httpContext.User.Identity)?.Claims.ToList();
        var role = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        return role == "Admin";
    }

    public bool IsAuthenticated(HttpContext httpContext, string id)
    {
        var userClaims = ((ClaimsIdentity)httpContext.User.Identity)?.Claims.ToList();
        var role = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userId  = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
        return (role == "User" && userId == id) || role == "Admin";
        

    }

    public bool IsViewer(HttpContext httpContext)
    {
        var userClaims = ((ClaimsIdentity)httpContext.User.Identity)?.Claims.ToList();
        var role = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        return role == "Viewer";
    }

    public string GetAuthenticatedId(HttpContext httpContext)
    {
        var userClaims = ((ClaimsIdentity)httpContext.User.Identity)?.Claims.ToList();
        var userId  = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
        return userId;
    }
}