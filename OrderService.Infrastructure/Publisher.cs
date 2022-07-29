using Common.Events;
using MassTransit;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure;

public class Publisher : IPublisher
{
    private readonly IBus _bus;

    public Publisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishOrderCreatedEvent(Order order)
    {
        await Task.Run(() =>
        {
            _bus.Publish<IOrderCreated>(new
            {
                OrderId = order.Id,
                LogMessage = $"New order added with id {order.Id}",
                order.CreatedAt
            });
        });
    }

    public async Task PublishOrderUpdatedEvent(Order order)
    {
        await Task.Run(() =>
        {
            _bus.Publish<IOrderUpdated>(new
            {
                OrderId= order.Id,
                LogMessage =$"Order with id {order.Id} updated.",
                order.UpdatedAt
            });
        });
    }
}