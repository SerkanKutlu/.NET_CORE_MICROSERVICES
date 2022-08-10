using System.Security.Claims;
using Core;
using Core.Entity;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;

    public DocumentService(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task Download(){}
    

    public async Task Delete(string docId)
    {
        
    }

    public async Task<string> Upload(HttpContext httpContext)
    {
        //Getting user informations
        var userClaims = ((ClaimsIdentity)httpContext.User.Identity)?.Claims.ToList();
        var role = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userId  = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
        
        //Getting file(s)
        var formCollection = await httpContext.Request.ReadFormAsync();
        var file = formCollection.Files.First();
        if (file.Length > 0)
        {
            var savingFolder = "Documents";
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), $"{savingFolder}\\{file.FileName}");
            var mimeType = pathToSave.GetMimeType();
            mimeType.CheckSupportedType();
            var document = new Document
            {
                FileName = $"{file.FileName}_uploadedBy_{role}",
                Id = Guid.NewGuid().ToString(),
                MimeType = mimeType,
                OriginalFileName = file.FileName,
                Path = pathToSave,
                UploadedAt = DateTime.Now,
                UserId = userId
            };
            await using (var stream = new FileStream(pathToSave, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _documentRepository.CreateAsync(document);
            return document.Id;
        }

        throw new DocumentNotSelectedException();
    }
}