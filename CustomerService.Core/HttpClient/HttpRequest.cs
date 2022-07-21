namespace CustomerService.Core.HttpClient;

public class HttpRequest : IHttpRequest
{
    private readonly IHttpClientFactory _factory;
    private readonly IHttpClientProperty _httpClientProperty;
    
    public HttpRequest(IHttpClientFactory factory, IHttpClientProperty httpClientProperty)
    {
        _factory = factory;
        _httpClientProperty = httpClientProperty;
    }

    public async Task<HttpResponseMessage> DeleteOrders(string customerId)
    {
        var url = $"{_httpClientProperty.DeleteOrderUrl}/{customerId}";
        return await _factory.CreateClient("httpClient").DeleteAsync(url);
    }
}