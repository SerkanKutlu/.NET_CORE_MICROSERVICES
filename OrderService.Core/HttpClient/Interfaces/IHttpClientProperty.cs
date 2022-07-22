using OrderService.Entity.Models;

namespace OrderService.Core.HttpClient.Interfaces;

public interface IHttpClientProperty
{
    public string GetAddressUrl { get; set; }
    public string ValidateCustomerUrl { get; set; }
}