namespace RedisSub.Interfaces;

public interface IRedisSubscriber
{
    public Task Subscribe();
}