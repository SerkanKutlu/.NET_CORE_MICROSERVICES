namespace CustomerService.Data.Settings;

public interface IMongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
}