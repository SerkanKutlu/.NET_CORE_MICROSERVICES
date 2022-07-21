namespace CustomerService.Core;

public interface IHttpRequest
{
    Task<HttpResponseMessage> DeleteOrders(string customerId);
}