using MongoDB.Driver;
using UserService.Data.Entity;
using UserService.Data.Exceptions;
using UserService.Data.Mongo;

namespace UserService.Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly IMongoService _mongoService;

    public UserRepository(IMongoService mongoService)
    {
        _mongoService = mongoService;
    }

    public async Task LoginAsync(string email, string password)
    {
        var user = await _mongoService.Users.Find(user => user.Email == email).FirstOrDefaultAsync();
        if (user == null) throw new NotFoundException<User>();
        if (user.Password != password) throw new InvalidPasswordException();
    }

    public async Task RegisterAsync(User user)
    {
        await _mongoService.Users.InsertOneAsync(user);
    }
}