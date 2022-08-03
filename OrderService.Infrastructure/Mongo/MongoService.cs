using MongoDB.Driver;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Mongo;

public class MongoService : IMongoService
{
    public IMongoCollection<Order> Orders { get; set; }
    public IMongoCollection<Product> Products { get; set; }


    public MongoService(IMongoSettings mongoSettings)
    {
        var mongoClient = new MongoClient(mongoSettings.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoSettings.DatabaseName);
        Orders = mongoDatabase.GetCollection<Order>(mongoSettings.CollectionNames[nameof(Order)]);
        Products = mongoDatabase.GetCollection<Product>(mongoSettings.CollectionNames[nameof(Product)]);

    }
}