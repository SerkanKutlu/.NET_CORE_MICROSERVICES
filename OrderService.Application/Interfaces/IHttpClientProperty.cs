namespace OrderService.Application.Interfaces;

public interface IHttpClientProperty
{
    public string GetAddressUrl { get; set; }
    public string ValidateCustomerUrl { get; set; }
}