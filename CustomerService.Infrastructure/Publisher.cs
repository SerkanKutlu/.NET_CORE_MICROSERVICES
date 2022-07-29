using Common.Commands;
using Common.Events;
using CustomerService.Application.Interfaces;
using CustomerService.Domain.Entities;
using MassTransit;

namespace CustomerService.Infrastructure;

public class Publisher:IPublisher
{
    private readonly IBus _bus;

    public Publisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishCustomerCreatedEvent(Customer customer)
    {
        await Task.Run(() =>
        {
            _bus.Publish<ICustomerCreated>(new
            {
                CustomerId = customer.Id,
                LogMessage = $"New customer added with id {customer.Id}",
                customer.CreatedAt
            });
        });
    }

    public async Task PublishCustomerUpdatedEvent(Customer customer)
    {
        await Task.Run(() =>
        {
            _bus.Publish<ICustomerUpdated>(new
            {
                CustomerId = customer.Id,
                LogMessage =$"Customer with id {customer.Id} updated.",
                customer.UpdatedAt
            });
        });
    }

    public async Task SendDeleteCustomerRelatedOrdersCommand(string customerId)
    {
        var uri = new Uri("rabbitmq://localhost/customer.delete");
        var ep = await _bus.GetSendEndpoint(uri);
        await ep.Send<IDeleteCustomerRelatedOrders>(new
        {
            CustomerId = customerId
        });
    }
}