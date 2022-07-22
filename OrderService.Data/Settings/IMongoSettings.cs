namespace OrderService.Data.Settings;

public interface IMongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public Dictionary<string, string> CollectionNames { get; set; }
}