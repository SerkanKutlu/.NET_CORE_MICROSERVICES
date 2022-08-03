
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Entities;

public class Order
{
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public int Quantity { get; set; } //This will be set auto
    public double Total { get; set; } //This will be set auto
    public string Status { get; set; }
    public Address Address { get; set; }
    public List<string> ProductIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}