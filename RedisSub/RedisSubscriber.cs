using System.Text.Json;
using CustomerService.Application.Events;
using CustomerService.Application.Interfaces;
using RedisSub.Interfaces;
using RedisSub.Models;
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
            _messageBatch.Add(msg);
            var customerCreated = JsonSerializer.Deserialize<CustomerCreated>(msg!);
            var log = new Log();
            log.FillLogMessage(customerCreated);
            _logBatch.Add(log);
            if (_logBatch.Count >= 5)
            {
                try
                {
                    _logRepository.AddRangeAsync(_logBatch);
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
            Thread.Sleep(5000);
            var customerCreated = JsonSerializer.Deserialize<CustomerCreated>(msg!);
            var log = new Log();
            log.FillLogMessage(customerCreated);
            _logRepository.AddAsync(log);
        });
    }
}