namespace CustomerService.Application.Interfaces;

public interface IRedisSettings
{
    public string ConnectionString { get; set; }
    public string ChannelName { get; set; }
}