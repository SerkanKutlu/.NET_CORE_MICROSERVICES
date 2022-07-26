using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IOrderHelper
{
    Task CheckCustomer(string customerId);
    Task SetAddressOfOrder(Order newOrder);
    Task SetTotalAmount(Order newOrder);
    Task<Order> SetPersistentDataForUpdate(Order orderForUpdate);
}