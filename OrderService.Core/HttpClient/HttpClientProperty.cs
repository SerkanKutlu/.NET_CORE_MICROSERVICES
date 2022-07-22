using OrderService.Core.HttpClient.Interfaces;

namespace OrderService.Core.HttpClient;

public class HttpClientProperty : IHttpClientProperty
{
    public string GetAddressUrl { get; set; }
    public string ValidateCustomerUrl { get; set; }
}