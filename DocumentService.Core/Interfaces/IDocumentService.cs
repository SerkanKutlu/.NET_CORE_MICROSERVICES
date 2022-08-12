using Core.Dto;
using Core.Entity;
using Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Interfaces
{
    public interface IDocumentService
    {
        Task<UploadResultDto> Upload(HttpContext httpContext);
        Task Delete(string docId,HttpContext httpContext);
        Task<FileContentResult> DownloadAllFiles(HttpContext httpContext);
        Task<FileContentResult> DownloadById(string id,HttpContext httpContext);
        Task<IEnumerable<DocumentEntity>> ShowAll(HttpContext httpContext);
    }
}