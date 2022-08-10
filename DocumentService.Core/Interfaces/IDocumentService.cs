using Core.Dto;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IDocumentService
{
    Task<UploadResultDto> Upload(HttpContext httpContext);
    Task Delete(string docId);
    Task Download();
}