using GenericMongo.Interfaces;

namespace GenericMongo;

public class MongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
}