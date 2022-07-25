using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using UserService.Data.DTO;
using UserService.Data.Entity;
using UserService.Data.Mongo;
using UserService.Data.Repository;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly ITokenHandler _tokenHandler;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(ITokenHandler tokenHandler, IUserRepository userRepository, IMapper mapper)
    {
        _tokenHandler = tokenHandler;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userLogin)
    {
        await _userRepository.LoginAsync(userLogin.Email, userLogin.Password);
        var token = _tokenHandler.CreateToken();
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userRegister)
    {
        var user = _mapper.Map<User>(userRegister);
        _userRepository.RegisterAsync(user);
        return Ok();
    }
}