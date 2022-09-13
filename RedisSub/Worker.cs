using RedisSub.Interfaces;

namespace RedisSub;

public class Worker : BackgroundService
{

    private readonly IRedisSubscriber _redisSubscriber;
    public Worker( IRedisSubscriber redisSubscriber)
    {
        _redisSubscriber = redisSubscriber;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _redisSubscriber.Subscribe();
    }
}