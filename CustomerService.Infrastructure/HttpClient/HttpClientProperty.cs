using CustomerService.Application.Interfaces;

namespace CustomerService.Infrastructure.HttpClient;

public class HttpClientProperty : IHttpClientProperty
{
    public string DeleteOrderUrl { get; set; }
}