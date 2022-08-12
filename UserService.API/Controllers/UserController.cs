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
    private readonly ITokenRepository _tokenRepository;
    private readonly IMapper _mapper;

    public UserController(ITokenHandler tokenHandler, IUserRepository userRepository, IMapper mapper, ITokenRepository tokenRepository)
    {
        _tokenHandler = tokenHandler;
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenRepository = tokenRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userLogin)
    {
        var user = await _userRepository.LoginAsync(userLogin.Email, userLogin.Password);
        var token = _tokenHandler.CreateToken(user);
        await _tokenRepository.DeleteAsync(t => t.UserId == user.Id);
        await _tokenRepository.AddAsync(token);
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

    [HttpPut("role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserRole([FromBody] RoleChangeDto roleChangeDto)
    {
        await _userRepository.UpdateUserRole(roleChangeDto.Id, roleChangeDto.Role);
        return Ok();
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] UserForPasswordUpdateDto passwordChangeDto)
    {
        var user = await _userRepository.LoginAsync(passwordChangeDto.Email, passwordChangeDto.OldPassword);
        await _userRepository.UpdateUserPassword(user, passwordChangeDto.NewPassword);
        await _tokenRepository.SetTokenInvalidate(user.Id);
        return Ok();
    }

    [HttpGet("validate/token/{id}")]
    public async Task<IActionResult> ValidateToken(string id)
    {
        await _tokenRepository.ValidateToken(id);
        return Ok();
    }
}