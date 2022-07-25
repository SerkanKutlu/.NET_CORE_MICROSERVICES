using MongoDB.Driver;
using UserService.Data.Entity;

namespace UserService.Data.Mongo;

public class MongoService :IMongoService
{
    public IMongoCollection<User> Users { get; set; }
    
    public MongoService(IMongoSettings mongoSettings)
    {
        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        Users = database.GetCollection<User>(mongoSettings.CollectionName);
        SeedUserData.SeedData(Users);
        //Unique Email Area
        
        var options = new CreateIndexOptions {Unique = true};
        var indexModel = new CreateIndexModel<User>("{Email:1}",options);
        Users.Indexes.CreateOne(indexModel);
    }
}