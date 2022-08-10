using System.IO.Compression;
using System.Security.Claims;
using Core;
using Core.Dto;
using Core.Entity;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    public DocumentService(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<FileContentResult> DownloadAllFiles(HttpContext httpContext)
    { 
        var userClaims = ((ClaimsIdentity)httpContext.User.Identity)?.Claims.ToList();
        var role = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userId  = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
        var documents = await _documentRepository.GetAllPathsAsync();
        using (var memoryStream = new MemoryStream())
        {
            using (var archieve = new ZipArchive(memoryStream, ZipArchiveMode.Update))
            {
                foreach (var document in documents)
                {
                    if(role!="Admin" && document.UserId != userId)
                        continue;
                    archieve.CreateEntryFromFile(document.Path, document.FileName);
                    
                }

                if (archieve.Entries.Count == 0)
                    throw new DocumentNotFoundException();
            }

            return new FileContentResult(memoryStream.ToArray(), "application/zip")
            {
                FileDownloadName = "docs.zip"
            };
        }
    }
    
    public async Task<FileContentResult> DownloadById(string id,HttpContext httpContext)
    {
        //Getting user informations
        var userClaims = ((ClaimsIdentity)httpContext.User.Identity)?.Claims.ToList();
        var role = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userId  = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
        var document = await _documentRepository.GetPathByIdAsync(id);
        if (userId != document.UserId && role !="Admin")
        {
            throw new UnAuthorizedRequest("User does not have an access to this file");
        }
        var data = await File.ReadAllBytesAsync(document.Path);
        var result = new FileContentResult(data, "application/octet-stream")
        {
            FileDownloadName = document.FileName
        };
        return result;
    }
    

    public async Task Delete(string docId,HttpContext httpContext)
    {
        //Getting user informations
        var userClaims = ((ClaimsIdentity)httpContext.User.Identity)?.Claims.ToList();
        var role = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userId  = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
        var document = await _documentRepository.GetPathByIdAsync(docId);
        if (role != "Admin" && userId != document.UserId)
        {
            throw new UnAuthorizedRequest("User does not have an access to this file");
        }
        File.Delete(document.Path);
        await _documentRepository.DeleteAsync(docId);
    }

    public async Task<UploadResultDto> Upload(HttpContext httpContext)
    {
        //Getting user informations
        var userClaims = ((ClaimsIdentity)httpContext.User.Identity)?.Claims.ToList();
        var role = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userId  = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
        
        //Getting file(s)
        var formCollection = await httpContext.Request.ReadFormAsync();
        var files = formCollection.Files;
        var createdDocuments = new Dictionary<string, string>();
        
        //Check if any unsupported types exist before saving.
        foreach (var file in files)
            file.FileName.GetMimeType().CheckSupportedType();
        
        //Upload Files
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var savingFolder = "Documents";
                //Adding timeStamp to file name
                var date = new DateTimeOffset(DateTime.UtcNow);
                var unixTime = date.ToUnixTimeSeconds().ToString();
                //Setting path
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), $"{savingFolder}\\{unixTime}_{file.FileName}");
                //Checking supported mime types
                var mimeType = pathToSave.GetMimeType();
                mimeType.CheckSupportedType();
                var document = new DocumentEntity
                {
                    FileName = $"{unixTime}_{file.FileName}_uploadedBy_{role}.pdf",
                    Id = Guid.NewGuid().ToString(),
                    MimeType = mimeType,
                    OriginalFileName = file.FileName,
                    UploadedAt = unixTime,
                    UserId = userId,
                    Path = pathToSave
                };
                if (mimeType.CheckIfImage())
                {
                    //Convert Image to pdf
                    Helper.CovertToPdfAndSave(file,pathToSave);
                    document.Path += ".pdf";
                }
                else
                {
                    await using var stream = new FileStream(pathToSave, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
                await _documentRepository.CreateAsync(document);
                createdDocuments.Add(file.FileName,document.Id);
            }
        }
        if (!createdDocuments.Any())
        {
            throw new DocumentNotSelectedException();
        }
        return new UploadResultDto
        {
            UploadedDocuments = createdDocuments
        };
        
    }
}