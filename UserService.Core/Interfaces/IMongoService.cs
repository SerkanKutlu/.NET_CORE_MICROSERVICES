using MongoDB.Driver;
using UserService.Core.Entity;

namespace UserService.Core.Interfaces;

public interface IMongoService
{
    IMongoCollection<User> Users { get; set; }
}