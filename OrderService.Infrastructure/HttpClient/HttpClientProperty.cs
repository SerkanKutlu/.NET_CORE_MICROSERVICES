using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.HttpClient;

public class HttpClientProperty : IHttpClientProperty
{
    public string GetAddressUrl { get; set; }
    public string ValidateCustomerUrl { get; set; }
}