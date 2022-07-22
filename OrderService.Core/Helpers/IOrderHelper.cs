using OrderService.Entity.Models;

namespace OrderService.Core.Helpers;

public interface IOrderHelper
{
    Task CheckCustomer(string customerId);
    Task SetAddressOfOrder(Order newOrder);
    Task SetTotalAmount(Order newOrder);
    Task<Order> SetPersistentDataForUpdate(Order orderForUpdate);
}