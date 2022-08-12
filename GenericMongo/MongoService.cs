using GenericMongo.Bases;
using GenericMongo.Interfaces;
using MongoDB.Driver;

namespace GenericMongo;

public class MongoService<T>:IMongoService<T> where T:BaseEntity
{
    public IMongoCollection<T> Collection { get; set; }

    public MongoService(MongoSettings setting)
    {
        var client = new MongoClient(setting.ConnectionString);
        var database = client.GetDatabase(setting.DatabaseName);
        Collection = database.GetCollection<T>(setting.CollectionName);
    }
}