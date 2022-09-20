using CustomerService.Application.Dto;
using StackExchange.Redis;

namespace CustomerService.Application.Interfaces;

public interface IRedisPublisher
{
    Task Publish(CustomerForLogDto customerForLogDto);
    Task PublishToRetry(RedisValue msg);
}