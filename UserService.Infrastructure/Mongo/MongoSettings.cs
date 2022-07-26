using UserService.Core.Interfaces;

namespace UserService.Infrastructure.Mongo;

public class MongoSettings : IMongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
}