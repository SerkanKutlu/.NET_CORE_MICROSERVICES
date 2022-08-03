using OrderService.Domain.Entities;

namespace OrderService.Application.DTO;

public class OrderForCreationDto
{
    public string CustomerId { get; set; }
    public string Status { get; set; }
    public List<string> ProductIds { get; set; }
    
    public Order ToOrder()
    {
        return new Order
        {
            CustomerId = CustomerId,
            Status = Status,
            ProductIds = ProductIds,
            CreatedAt = DateTime.UtcNow,
            Id = Guid.NewGuid().ToString()
        };
    }
}