namespace CustomerService.Core.HttpClient.Interfaces;

public interface IHttpRequest
{
    Task<HttpResponseMessage> DeleteOrders(string customerId);
}