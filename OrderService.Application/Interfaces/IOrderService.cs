using Microsoft.AspNetCore.Http;
using OrderService.Application.DTO;
using OrderService.Application.Models;
using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IOrderService
{
    Task<PagedList<Order>> GetPagedOrders(RequestParameters requestParameters,HttpContext context);
    Task<Order> GetById(string id);
    Task<PagedList<Order>> GetOrdersOfCustomersPaged(string customerId, RequestParameters requestParameters,HttpContext context);
    Task<string> CreateOrder(OrderForCreationDto newOrder,HttpContext context);
    Task<Order> UpdateOrder(OrderForUpdateDto newOrder);
    Task UpdateStatus(string id, string newStatus);
    Task DeleteOrder(string id);
    Task DeleteOrderOfCustomer(string customerId);




}