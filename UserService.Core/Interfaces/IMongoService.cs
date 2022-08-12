using MongoDB.Driver;
using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Core.Interfaces;

public interface IMongoService
{
    IMongoCollection<User> Users { get; set; }
    IMongoCollection<Token> Tokens { get; set; }
}