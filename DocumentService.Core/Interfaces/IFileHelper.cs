using Core.Entity;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IFileHelper
{
    string GetMimeType(string filePath);
    bool CheckIfImage(string mimeType);
    void CheckSupportedType(IFormFileCollection files);
    void CovertToPdfAndSave(IFormFile image, string pathToSave);
    Task<DocumentEntity> UploadFiles(IFormFile file, HttpContext httpContext);
}