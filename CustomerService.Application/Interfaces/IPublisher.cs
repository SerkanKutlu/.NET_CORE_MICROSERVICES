using CustomerService.Domain.Entities;

namespace CustomerService.Application.Interfaces;

public interface IPublisher
{
    Task PublishCustomerCreatedEvent(Customer customer);
    Task PublishCustomerUpdatedEvent(Customer customer);
    Task SendDeleteCustomerRelatedOrdersCommand(string customerId);
}