using System.Text.Json;
using CustomerService.Application.Dto;
using CustomerService.Application.Interfaces;
using RedisSub.Entities;
using RedisSub.Interfaces;
using RedisSub.Repositories;
using StackExchange.Redis;

namespace RedisSub;

public class RedisSubscriber : IRedisSubscriber
{
    private readonly IRedisService _redisService;
    private readonly LogRepository _logRepository;
    private readonly IRedisPublisher _redisPublisher;
    private readonly List<Log> _logBatch = new();
    private readonly List<RedisValue> _messageBatch = new();
    
    public RedisSubscriber(IRedisService redisService, LogRepository logRepository, IRedisPublisher redisPublisher)
    {
        _redisService = redisService;
        _logRepository = logRepository;
        _redisPublisher = redisPublisher;
    }
    public async Task Subscribe()
    {
        await _redisService.Subscriber.SubscribeAsync(_redisService.RedisSettings.ChannelName, (channel, msg) =>
        {
            Console.WriteLine("message at main channel");
            _messageBatch.Add(msg);
            var dto = JsonSerializer.Deserialize<CustomerForLogDto>(msg!);
            var log = new Log
            {
                Id = Guid.NewGuid().ToString(),
                LogMessage = $"Customer was {dto?.Action}.  {dto}",
                CreatedAt = DateTime.UtcNow
            };
            _logBatch.Add(log);
            if (_logBatch.Count >= 5)
            {
                try
                {
                    //_logRepository.AddRangeAsync(_logBatch);
                    throw new Exception("xx");

                }
                catch (Exception e)
                {
                    foreach (var eMsg in _messageBatch)
                    {
                        _redisPublisher.PublishToRetry(eMsg);
                    }
                }
                
                _logBatch.Clear();
                _messageBatch.Clear();
            }

        });

        await _redisService.Subscriber.SubscribeAsync(_redisService.RedisSettings.RetryChannel, (channel, msg) =>
        {
            Console.WriteLine("message at retry channel");
            Thread.Sleep(5000);
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