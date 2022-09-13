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

    [HttpGet("Show")]
    [Authorize(Roles="Admin,User,Viewer")]
    public async Task<IActionResult> ShowAll()
    {
        return Ok("role based gecildi");
        var result =await _documentService.ShowAll(HttpContext);
        return Ok(result);
    }

    [HttpGet("Download/All")]
    [Authorize(Roles = "Admin,Viewer,User")]
    public async Task<FileContentResult> DownloadAll()
    {
        var result = await _documentService.DownloadAllFiles(HttpContext);
        return result;

    }

    [Authorize(Roles = "Admin,User,Viewer")]
    [HttpGet("Download")]
    public async Task<IActionResult> DownloadById([FromQuery]string id)
    {
        var result =await _documentService.DownloadById(id,HttpContext);
        return result;
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Delete([FromQuery]string id)
    {
        await _documentService.Delete(id, HttpContext);
        return Ok();
    }
    

    [HttpPost("Upload")]
    [Authorize(Roles = "Admin,User")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Upload()
    {
        var result =await _documentService.Upload(HttpContext);
        return Ok(result);
    }
}