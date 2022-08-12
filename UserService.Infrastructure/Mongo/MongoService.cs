using MongoDB.Driver;
using UserService.Core.Entity;
using UserService.Core.Interfaces;
using UserService.Core.Models;

namespace UserService.Infrastructure.Mongo;

public class MongoService :IMongoService
{
    public IMongoCollection<User> Users { get; set; }
    public IMongoCollection<Token> Tokens { get; set; }

    public MongoService(IMongoSettings mongoSettings)
    {
        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        Users = database.GetCollection<User>(mongoSettings.CollectionName);
        Tokens = database.GetCollection<Token>("TokenCollection");
        SeedUserData.SeedData(Users);
        //Unique Email Area
        
        var options = new CreateIndexOptions {Unique = true};
        var indexModel = new CreateIndexModel<User>("{Email:1}",options);
        Users.Indexes.CreateOne(indexModel);
    }
}