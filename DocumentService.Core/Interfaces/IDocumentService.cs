using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IDocumentService
{
    Task<string> Upload(HttpContext httpContext);
    Task Delete(string docId);
    Task Download();
}