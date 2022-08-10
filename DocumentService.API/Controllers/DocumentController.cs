using System.IO.Compression;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;
    public DocumentController(IDocumentService documentService)
    {
        _documentService = documentService;
    }


    [HttpGet("download")]
    [Authorize(Roles = "Admin,Viewer,User")]
    public async Task<FileContentResult> Download()
    {
        var result = await _documentService.DownloadAllFiles(HttpContext);
        return result;

    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet("download/{id}")]
    public async Task<FileContentResult> DownloadById(string id)
    {
        var result =await _documentService.DownloadById(id,HttpContext);
        return result;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Delete(string id)
    {
        await _documentService.Delete(id, HttpContext);
        return Ok();
    }
    
    
    

    [HttpPost("upload")]
    //[Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> Upload()
    {
        var result =await _documentService.Upload(HttpContext);
        return Ok(result);
    }
}