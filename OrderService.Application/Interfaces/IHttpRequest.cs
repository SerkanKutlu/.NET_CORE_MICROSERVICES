namespace OrderService.Application.Interfaces;

public interface IHttpRequest
{
    Task<HttpResponseMessage> GetCustomerAddressAsync(string customerId);
    Task<HttpResponseMessage> ValidateCustomerAsync(string customerId);
}