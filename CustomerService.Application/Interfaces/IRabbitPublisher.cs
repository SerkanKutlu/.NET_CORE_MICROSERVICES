using CustomerService.Application.Events;
using RabbitMQ.Client;

namespace CustomerService.Application.Interfaces;

public interface IRabbitPublisher
{
    void Publish(CustomerCreated customerCreated);
}