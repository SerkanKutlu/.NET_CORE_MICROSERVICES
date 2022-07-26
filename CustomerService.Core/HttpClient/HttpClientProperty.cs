using CustomerService.Core.HttpClient.Interfaces;

namespace CustomerService.Core.HttpClient;

public class HttpClientProperty : IHttpClientProperty
{
    public string DeleteOrderUrl { get; set; }
}