using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    

    public CustomersController(ILogger<CustomersController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult denemeGt()
    {
        _logger.LogInformation("LOG FROM GET ENDPOINT");
        return Ok();
    }


}