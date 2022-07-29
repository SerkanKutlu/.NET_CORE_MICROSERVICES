using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IPublisher
{
    Task PublishOrderCreatedEvent(Order order);
    Task PublishOrderUpdatedEvent(Order order);
}