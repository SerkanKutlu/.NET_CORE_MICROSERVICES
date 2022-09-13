using System.Text.Json;
using CustomerService.Application.Dto;
using CustomerService.Application.Interfaces;
using StackExchange.Redis;

namespace CustomerService.Infrastructure.Redis;

public class RedisService : IRedisService
{

    public IRedisSettings RedisSettings { get; set; }
    public ISubscriber Subscriber { get; set; }
    public RedisService(IRedisSettings redisSettings)
    {
        RedisSettings = redisSettings;
        var connection = ConnectionMultiplexer.Connect(RedisSettings.ConnectionString);
        Subscriber = connection.GetSubscriber();
    }
    
    
}