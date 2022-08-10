using Core.Entity;
using Core.Interfaces;
using MongoDB.Driver;

namespace DocumentService.Infrastructure.Mongo;

public class MongoService : IMongoService
{
    
    public IMongoCollection<DocumentEntity> Documents { get; set; }
    
    public MongoService(IMongoSettings mongoSettings)
    {
        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        Documents = database.GetCollection<DocumentEntity>(mongoSettings.CollectionName);
    }
   
}