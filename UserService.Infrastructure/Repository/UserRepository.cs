using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using UserService.Core.Entity;
using UserService.Core.Exceptions;
using UserService.Core.Interfaces;
using UserService.Core.Models;

namespace UserService.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly IMongoService _mongoService;
    private readonly IPasswordHasher<User> _passwordHasher;
    public UserRepository(IMongoService mongoService, IPasswordHasher<User> passwordHasher)
    {
        _mongoService = mongoService;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> LoginAsync(string email, string password)
    {
        var user = await _mongoService.Users.Find(user => user.Email == email).FirstOrDefaultAsync();
        if (user == null) throw new NotFoundException<User>();
        if (!BCrypt.Net.BCrypt.Verify(password,user.Password))
            throw new InvalidPasswordException();
        return user;
    }

    public async Task RegisterAsync(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await _mongoService.Users.InsertOneAsync(user);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _mongoService.Users.Find(u => true).ToListAsync();
    }

    public async Task UpdateUserRole(string id, string newRole)
    {
        var result = await _mongoService.Users.UpdateOneAsync(
            u => u.Id == id,
            Builders<User>.Update
                .Set(u => u.Role, newRole));

        if (!result.IsModifiedCountAvailable && result.ModifiedCount == 0)
            throw new NotFoundException<User>(id);
    }

    public async Task UpdateUserPassword(User user, string newPassword)
    {
        user.Password = _passwordHasher.HashPassword(user, newPassword);
        var result = await _mongoService.Users.FindOneAndReplaceAsync(u=>u.Id == user.Id,user);
        if (result == null)
        {
            throw new NotFoundException<User>();
        }
    }

    public async Task UpdateUser(User user)
    {
        var result = await _mongoService.Users.FindOneAndReplaceAsync(u=>u.Id == user.Id,user);
        if (result == null)
        {
            throw new NotFoundException<User>();
        }
    }

    public async Task ValidateToken(string id)
    {
        var user = await _mongoService.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
        if (user == null)
            throw new NotFoundException<User>();
        if (user.PasswordChanged)
            throw new InvalidTokenException();
        
    }
}