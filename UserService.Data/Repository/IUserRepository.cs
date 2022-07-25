using UserService.Data.Entity;

namespace UserService.Data.Repository;

public interface IUserRepository
{
    Task LoginAsync(string email, string password);
    Task RegisterAsync(User user);
}