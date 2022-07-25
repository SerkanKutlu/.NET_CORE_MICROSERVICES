using CustomerService.Domain.Entities;

namespace CustomerService.Application.Interfaces;

public interface ICustomerHelper
{
    Task SetCreatedAt(Customer customerForUpdate);
    Task DeleteRelatedOrders(string customerId);
    Task StartDeleteChain(string customerId);
}