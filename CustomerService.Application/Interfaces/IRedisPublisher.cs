using CustomerService.Application.Dto;

namespace CustomerService.Application.Interfaces;

public interface IRedisPublisher
{
    Task Publish(CustomerForLogDto customerForLogDto);
}