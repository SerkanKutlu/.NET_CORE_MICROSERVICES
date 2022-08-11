using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IAuthHelper
{
    bool IsAuthenticated(HttpContext httpContext);
    bool IsAuthenticated(HttpContext httpContext, string id);
    bool IsViewer(HttpContext httpContext);
    string GetAuthenticatedId(HttpContext httpContext);
}