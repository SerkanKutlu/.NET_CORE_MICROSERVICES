using OrderService.Consumer.Interfaces;

namespace OrderService.Consumer.Mongo;

public class MongoSettings : IMongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
}