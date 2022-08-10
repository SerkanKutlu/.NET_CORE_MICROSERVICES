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


    [HttpGet]
    [Authorize(Roles = "Admin,Viewer")]
    public async Task<IActionResult> Download()
    {
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string docId)
    {
        return Ok();
    }
    
    
    

    [HttpPost,DisableRequestSizeLimit]
    //[Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> Upload()
    {
        var result =await _documentService.Upload(HttpContext);
        return Ok(result);
    }
}