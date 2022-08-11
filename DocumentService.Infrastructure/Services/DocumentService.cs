using System.IO.Compression;
using Core.Dto;
using Core.Entity;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IFileHelper _fileHelper;
    private readonly IAuthHelper _authHelper;
    public DocumentService(IDocumentRepository documentRepository, IFileHelper fileHelper, IAuthHelper authHelper)
    {
        _documentRepository = documentRepository;
        _fileHelper = fileHelper;
        _authHelper = authHelper;
    }

    public async Task<FileContentResult> DownloadAllFiles(HttpContext httpContext)
    { 
        var documents = await _documentRepository.GetAllPathsAsync();
        await using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
        {
            foreach (var document in documents)
            {
                if(!(_authHelper.IsAuthenticated(httpContext,document.UserId) || _authHelper.IsViewer(httpContext)))
                    continue;
                archive.CreateEntryFromFile(document.Path, document.FileName);
            }
            if(archive.Entries.Count==0)
                throw new DocumentNotFoundException();
              
        }
        return new FileContentResult(memoryStream.ToArray(), "application/zip")
        {
            FileDownloadName = "docs.zip"
        };
    }
    
    public async Task<FileContentResult> DownloadById(string id,HttpContext httpContext)
    {
        var document = await _documentRepository.GetPathByIdAsync(id);
        if (!(_authHelper.IsAuthenticated(httpContext,document.UserId) || _authHelper.IsViewer(httpContext)))
        {
            throw new UnAuthorizedRequest("User does not have an access to this file");
        }
        var data = await File.ReadAllBytesAsync(document.Path);
        var result = new FileContentResult(data, "application/pdf")
        {
            FileDownloadName = document.FileName
        };
        return result;
    }

    public async Task<IList<DocumentEntity>> ShowAll(HttpContext httpContext)
    {
        if (_authHelper.IsAuthenticated(httpContext) || _authHelper.IsViewer(httpContext))
            return await _documentRepository.GetEntities();
        return await _documentRepository.GetEntities(_authHelper.GetAuthenticatedId(httpContext));
    }


    public async Task Delete(string docId,HttpContext httpContext)
    {
        var document = await _documentRepository.GetPathByIdAsync(docId);
        if (!_authHelper.IsAuthenticated(httpContext,document.UserId))
        {
            throw new UnAuthorizedRequest("User does not have an access to this file");
        }
        File.Delete(document.Path);
        await _documentRepository.DeleteAsync(docId);
    }

    public async Task<UploadResultDto> Upload(HttpContext httpContext)
    {
        //Getting file(s)
        var formCollection = await httpContext.Request.ReadFormAsync();
        var files = formCollection.Files;
        var createdDocuments = new Dictionary<string, string>();

        //Check if any unsupported types exist before saving.
        _fileHelper.CheckSupportedType(files);
        //Upload Files
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var document = await _fileHelper.UploadFiles(file,httpContext);
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