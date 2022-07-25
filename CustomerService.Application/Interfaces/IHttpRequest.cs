namespace CustomerService.Application.Interfaces;

public interface IHttpRequest
{
    Task<HttpResponseMessage> DeleteOrders(string customerId);
}