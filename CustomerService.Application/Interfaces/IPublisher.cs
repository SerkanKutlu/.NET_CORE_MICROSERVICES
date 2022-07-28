using CustomerService.Domain.Entities;

namespace CustomerService.Application.Interfaces;

public interface IPublisher
{
    Task PublishAtCreation(Customer customer);
    Task PublishAtUpdate(Customer customer);
}