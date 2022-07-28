using CustomerService.Application.Interfaces;
using CustomerService.Domain.Entities;
using MassTransit;

namespace CustomerService.Infrastructure.Publishers;

public class LogPublisher:IPublisher
{
    private readonly IBus _bus;

    public LogPublisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishAtCreation(Customer customer)
    {
        var uri = new Uri("rabbitmq://localhost/createQueue");
        var endPoint = await _bus.GetSendEndpoint(uri);
        await endPoint.Send(customer);
    }
    public async Task PublishAtUpdate(Customer customer)
    {
        var uri = new Uri("rabbitmq://localhost/updateQueue");
        var endPoint = await _bus.GetSendEndpoint(uri);
        await endPoint.Send(customer);
    }
}