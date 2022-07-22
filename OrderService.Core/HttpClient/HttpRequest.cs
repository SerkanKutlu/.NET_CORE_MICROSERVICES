using OrderService.Core.HttpClient.Interfaces;
using OrderService.Entity.Models;

namespace OrderService.Core.HttpClient;

public class HttpRequest : IHttpRequest
{
    private readonly IHttpClientFactory _factory;
    private readonly IHttpClientProperty _httpClientProperty;
    
    public HttpRequest(IHttpClientFactory factory, IHttpClientProperty httpClientProperty)
    {
        _factory = factory;
        _httpClientProperty = httpClientProperty;
    }

    public async Task<HttpResponseMessage> GetCustomerAddressAsync(string customerId)
    {
        var url = $"{_httpClientProperty.GetAddressUrl}/{customerId}";
        return await _factory.CreateClient("httpClient").GetAsync(url);
    }

    public async Task<HttpResponseMessage> ValidateCustomerAsync(string customerId)
    {
        var url = $"{_httpClientProperty.ValidateCustomerUrl}/{customerId}";
        return await _factory.CreateClient("httpClient").GetAsync(url);
    }
}