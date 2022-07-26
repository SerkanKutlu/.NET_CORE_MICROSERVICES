using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.DTO;
using UserService.Core.Entity;
using UserService.Core.Interfaces;
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
        var user = await _userRepository.LoginAsync(userLogin.Email, userLogin.Password);
        var token = _tokenHandler.CreateToken(user);
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userRegister)
    {
        var user = _mapper.Map<User>(userRegister);
        await _userRepository.RegisterAsync(user);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> ListUsers()
    {
        var users = await _userRepository.GetAllUsers();
        return Ok(users);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserRole([FromBody] RoleChangeDto roleChangeDto)
    {
        await _userRepository.UpdateUserRole(roleChangeDto.Id, roleChangeDto.Role);
        return Ok();


    }
}