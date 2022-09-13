using CustomerService.Application.Interfaces;

namespace CustomerService.Infrastructure.Redis;

public class RedisSettings:IRedisSettings
{
    public string ConnectionString { get; set; }
    public string ChannelName { get; set; }
}