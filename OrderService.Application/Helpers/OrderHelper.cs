using System.Net;
using System.Net.Http.Json;
using OrderService.Application.Exceptions;
using OrderService.Application.Interfaces;
using OrderService.Application.Models;
using OrderService.Domain.Entities;

namespace OrderService.Application.Helpers;

public class OrderHelper:IOrderHelper
{
    private readonly IHttpRequest _httpRequest;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    public OrderHelper(IHttpRequest httpRequest, IProductRepository repository, IOrderRepository orderRepository)
    {
        _httpRequest = httpRequest;
        _productRepository = repository;
        _orderRepository = orderRepository;
    }

    public async Task CheckCustomer(string customerId)
    {
        var response =await _httpRequest.ValidateCustomerAsync(customerId);
        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException<CustomerModel>(customerId);
        if (response.StatusCode == HttpStatusCode.BadRequest)
            throw new InvalidModelException();
        if (response.StatusCode != HttpStatusCode.OK)
            throw new ServerNotRespondingException();
    }

    public async Task SetAddressOfOrder(Order newOrder)
    {
        var response =await _httpRequest.GetCustomerAddressAsync(newOrder.CustomerId);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var customerModel = await response.Content.ReadFromJsonAsync<CustomerModel>();
            if (customerModel?.Address != null) newOrder.Address = customerModel.Address;
        }
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException<CustomerModel>(newOrder.CustomerId);
        }
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new FormatException();
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new ServerNotRespondingException();
        }
    }

    public async Task SetTotalAmount(Order newOrder)
    {
        var total = 0d;
        foreach (var id in newOrder.ProductIds)
        {
            var product = await _productRepository.GetWithId(id);
            total += product.Price;
        }

        newOrder.Total = total;
    }
    
    public async Task<Order> SetPersistentDataForUpdate(Order orderForUpdate)
    {
        var oldOrder =await _orderRepository.GetWithId(orderForUpdate.Id);
        if (oldOrder == null)
            throw new NotFoundException<Order>(orderForUpdate.Id);
        orderForUpdate.CreatedAt = oldOrder.CreatedAt;
        orderForUpdate.CustomerId = oldOrder.CustomerId;
        return orderForUpdate;
    }
    
}