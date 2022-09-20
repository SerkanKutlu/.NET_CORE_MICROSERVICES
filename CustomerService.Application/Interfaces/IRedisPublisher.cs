using CustomerService.Application.Dto;
using CustomerService.Application.Events;
using StackExchange.Redis;

namespace CustomerService.Application.Interfaces;

public interface IRedisPublisher
{
    Task Publish(CustomerCreated customerForLogDto);
    Task PublishToRetry(RedisValue msg);
}