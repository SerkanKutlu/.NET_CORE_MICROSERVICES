using MongoDB.Driver;
using UserService.Data.Entity;

namespace UserService.Data.Mongo;

public interface IMongoService
{
    IMongoCollection<User> Users { get; set; }
}