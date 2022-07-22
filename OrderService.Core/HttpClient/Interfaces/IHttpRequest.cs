using OrderService.Entity.Models;

namespace OrderService.Core.HttpClient.Interfaces;

public interface IHttpRequest
{
    Task<HttpResponseMessage> GetCustomerAddressAsync(string customerId);
    Task<HttpResponseMessage> ValidateCustomerAsync(string customerId);
}