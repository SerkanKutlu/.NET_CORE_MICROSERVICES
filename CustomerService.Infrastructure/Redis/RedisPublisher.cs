using System.Text.Json;
using CustomerService.Application.Dto;
using CustomerService.Application.Interfaces;

namespace CustomerService.Infrastructure.Redis;

public class RedisPublisher : IRedisPublisher
{
    private readonly IRedisService _redisService;
    public RedisPublisher(IRedisService redisService)
    {
        _redisService = redisService;
    }
    public async Task Publish(CustomerForLogDto customerForLogDto)
    {
        var redisMessage = JsonSerializer.Serialize(customerForLogDto);
        await _redisService.Subscriber.PublishAsync(_redisService.RedisSettings.ChannelName, redisMessage);

    }
}