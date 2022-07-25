using CustomerService.Application.Interfaces;
using CustomerService.Domain.Entities;
using MongoDB.Driver;

namespace CustomerService.Infrastructure.Mongo;

public class MongoService : IMongoService
{
    
    public IMongoCollection<Customer> Customers { get; set; }
    
    public MongoService(IMongoSettings mongoSettings)
    {
        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        Customers = database.GetCollection<Customer>(mongoSettings.CollectionName);
        SeedCustomerData.SeedData(Customers);
        //Unique Email Area
        
        var options = new CreateIndexOptions {Unique = true};
        var indexModel = new CreateIndexModel<Customer>("{Email:1}",options);
        Customers.Indexes.CreateOne(indexModel);
    }
   
}