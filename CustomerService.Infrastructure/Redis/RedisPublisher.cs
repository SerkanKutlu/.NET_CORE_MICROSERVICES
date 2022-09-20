using System.Text.Json;
using CustomerService.Application.Events;
using CustomerService.Application.Interfaces;
using StackExchange.Redis;

namespace CustomerService.Infrastructure.Redis;

public class RedisPublisher : IRedisPublisher
{
    private readonly IRedisService _redisService;
    public RedisPublisher(IRedisService redisService)
    {
        _redisService = redisService;
    }
    public async Task Publish(CustomerCreated customerForLogDto)
    {
        var redisMessage = JsonSerializer.Serialize(customerForLogDto);
        var reachedCount = await _redisService.Subscriber.PublishAsync(_redisService.RedisSettings.ChannelName, redisMessage);
        Console.WriteLine(reachedCount);
    }

    public async Task PublishToRetry(RedisValue msg)
    {
        await _redisService.Subscriber.PublishAsync(_redisService.RedisSettings.RetryChannel, msg);
    } 
}