using CustomerService.Consumer.Interfaces;
using CustomerService.Consumer.Models;
using MongoDB.Driver;

namespace CustomerService.Consumer.Mongo;

public class MongoService : IMongoService
{
    
    public IMongoCollection<Log> Logs { get; set; }
    
    public MongoService(IMongoSettings mongoSettings)
    {
        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        Logs = database.GetCollection<Log>(mongoSettings.CollectionName);
    }
   
}