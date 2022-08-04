using MongoDB.Driver;
using OrderService.Consumer.Interfaces;
using OrderService.Consumer.Models;

namespace OrderService.Consumer.Mongo;

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