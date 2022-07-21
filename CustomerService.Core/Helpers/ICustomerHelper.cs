using CustomerService.Entity.Models;

namespace CustomerService.Core.Helpers;

public interface ICustomerHelper
{
    Task SetCreatedAt(Customer customerForUpdate);
    Task DeleteRelatedOrders(string customerId);
    Task StartDeleteChain(string customerId);
}