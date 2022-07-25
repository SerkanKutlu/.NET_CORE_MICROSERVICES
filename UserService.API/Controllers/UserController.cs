using Microsoft.AspNetCore.Mvc;
using UserService.API.DTO;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly ITokenHandler _tokenHandler;

    public UserController(ITokenHandler tokenHandler)
    {
        _tokenHandler = tokenHandler;
    }

    [HttpPost]
    public IActionResult Login([FromBody] UserForLoginDto userLogin)
    {
        return Ok(_tokenHandler.CreateToken());
    }
}