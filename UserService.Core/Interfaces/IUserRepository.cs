using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Core.Interfaces;

public interface IUserRepository
{
    Task<User> LoginAsync(string email, string password);
    Task RegisterAsync(User user);
    Task<IEnumerable<User>> GetAllUsers();
    Task UpdateUserRole(string id, string newRole);
    Task UpdateUserPassword(User user, string newPassword);

}