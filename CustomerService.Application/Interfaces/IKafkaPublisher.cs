using CustomerService.Application.Events;

namespace CustomerService.Application.Interfaces;

public interface IKafkaPublisher
{
    Task Publish(CustomerCreated customerCreated);
}