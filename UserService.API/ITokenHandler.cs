using UserService.API.Models;

namespace UserService.API;

public interface ITokenHandler
{
    Token CreateToken();
}