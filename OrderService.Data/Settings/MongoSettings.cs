namespace OrderService.Data.Settings;

public class MongoSettings:IMongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public Dictionary<string, string> CollectionNames { get; set; }
}