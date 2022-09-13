using System.Text.Json;
using CustomerService.Application.Dto;
using CustomerService.Application.Interfaces;
using RedisSub.Entities;
using RedisSub.Interfaces;
using RedisSub.Repositories;

namespace RedisSub;

public class RedisSubscriber : IRedisSubscriber
{
    private readonly IRedisService _redisService;
    private readonly LogRepository _logRepository;
    public RedisSubscriber(IRedisService redisService, LogRepository logRepository)
    {
        _redisService = redisService;
        _logRepository = logRepository;
    }
    public async Task Subscribe()
    {
        await _redisService.Subscriber.SubscribeAsync(_redisService.RedisSettings.ChannelName, (channel, msg) =>
        {
            var dto = JsonSerializer.Deserialize<CustomerForLogDto>(msg!);
            var log = new Log
            {
                Id = Guid.NewGuid().ToString(),
                LogMessage = $"Customer was {dto?.Action}.  {dto}",
                CreatedAt = DateTime.UtcNow
            };
            _logRepository.AddAsync(log);

        });
    }
}