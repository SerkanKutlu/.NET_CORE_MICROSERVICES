using OrderService.Application.Models;
using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IOrderRepository
{
    Task CreateAsync(Order newOrder);
    Task UpdateAsync(Order updatedOrder);
    Task DeleteAsync(string orderId);
    Task<PagedList<Order>> GetAll(RequestParameters orderParameters);
    Task<Order> GetWithId(string orderId);
    Task ChangeStatus(string orderId, string newStatus);
    Task<PagedList<Order>> GetOrdersOfCustomer(string customerId,RequestParameters requestParameters);
    Task<bool> DeleteOrderOfCustomer(string customerId);
    Task UpdateProductRelatedOrders(string productId);
}