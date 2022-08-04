using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Application.Interfaces;
using OrderService.Application.Models;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Services;

public class OrderService:IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderHelper _orderHelper;
    private readonly IPublisher _publisher;
    public OrderService(ILogger<OrderService> logger, IOrderRepository orderRepository, IOrderHelper orderHelper, IPublisher publisher)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _orderHelper = orderHelper;
        _publisher = publisher;
    }


    public async Task<PagedList<Order>> GetPagedOrders(RequestParameters requestParameters, HttpContext context)
    {
        var orders = await _orderRepository.GetAll(requestParameters);
        context.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(orders.MetaData));
        _logger.LogInformation("Getting all orders data's from database");
        return orders;
    }

    public async Task<Order> GetById(string id)
    {
        var order = await _orderRepository.GetWithId(id);
        _logger.LogInformation($"Getting order data with {id} from database");
        return order;
    }

    public async Task<PagedList<Order>> GetOrdersOfCustomersPaged(string customerId, RequestParameters requestParameters, HttpContext context)
    {
        var orders =await _orderRepository.GetOrdersOfCustomer(customerId,requestParameters);
        context.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(orders.MetaData));
        _logger.LogInformation($"Orders of customer with id {customerId} fetched");
        return orders;
    }

    public async Task<string> CreateOrder(OrderForCreationDto newOrder, HttpContext context)
    {
        var order = newOrder.ToOrder();
        await _orderHelper.SetTotalAmount(order);
        await _orderHelper.SetAddressOfOrder(order);
            
        await _orderRepository.CreateAsync(order);
        context.Response.Headers.Add("location",$"https://{context.Request.Headers["Host"]}/api/Orders/{order.Id}");
        _logger.LogInformation($"New order added with id {order.Id}");
        var orderForLogDto = new OrderForLogDto();
        orderForLogDto.FillWithOrder(order,"Created");
        _publisher.PublishForLog(orderForLogDto);
        return order.Id;
    }

    public async Task<Order> UpdateOrder(OrderForUpdateDto newOrder)
    {
        var order = newOrder.ToOrder();
        await _orderHelper.SetPersistentDataForUpdate(order);
        await _orderHelper.SetTotalAmount(order);
        await _orderRepository.UpdateAsync(order);
        _logger.LogInformation($"Order with id {order.Id} updated.");
        var orderForLogDto = new OrderForLogDto();
        orderForLogDto.FillWithOrder(order,"Updated");
        _publisher.PublishForLog(orderForLogDto);
        return order;
    }

    public async Task UpdateStatus(string id, string newStatus)
    {
        await _orderRepository.ChangeStatus(id, newStatus);
    }

    public async Task DeleteOrder(string id)
    {
        await _orderRepository.DeleteAsync(id);
        _logger.LogInformation($"Order with id {id} deleted.");
    }

    public async Task DeleteOrderOfCustomer(string customerId)
    {
        var result = await _orderRepository.DeleteOrderOfCustomer(customerId);
        if (!result) throw new NotFoundException<Order>();
        _logger.LogInformation($"Orders of customer with customer id {customerId} deleted.");
    }
}