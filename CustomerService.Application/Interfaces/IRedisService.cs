using CustomerService.Application.Dto;
using StackExchange.Redis;

namespace CustomerService.Application.Interfaces;

public interface IRedisService
{
    IRedisSettings RedisSettings { get; set; }
    ISubscriber Subscriber { get; set; }
}