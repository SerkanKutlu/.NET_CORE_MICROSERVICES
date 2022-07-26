using OrderService.Domain.ValueObjects;

namespace OrderService.Application.Models;

public class CustomerModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}