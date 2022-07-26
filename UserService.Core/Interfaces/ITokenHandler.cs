using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Core.Interfaces;

public interface ITokenHandler
{
    Token CreateToken(User user);
}