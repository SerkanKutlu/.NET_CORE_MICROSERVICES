using MongoDB.Driver;
using OrderService.Data.Settings;
using OrderService.Entity.Models;

namespace OrderService.Data.Mongo;

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
        SeedOrderData.SeedData(Orders);
        SeedProductData.SeedData(Products);
        
    }
}