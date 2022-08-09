using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using OrderService.Domain.Entities;
using OrderService.Domain.ValueObjects;

namespace OrderService.Application.DTO;

public class OrderForLogDto : ISerializer<OrderForLogDto>, IDeserializer<OrderForLogDto>
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
    public string Action { get; set; }
    
    public byte[] Serialize(OrderForLogDto data, SerializationContext context)
    {
        var serialized = JsonSerializer.Serialize(data);
        return Encoding.UTF8.GetBytes(serialized);
    }

    public OrderForLogDto Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var stringDto = Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<OrderForLogDto>(stringDto);
    }
    
    public void FillWithOrder(Order order, string action)
    { 
        Id = order.Id;
        CustomerId= order.CustomerId;
        Quantity= order.Quantity;
        Total= order.Total;
        Status= order.Status;
        Address= order.Address;
        ProductIds= order.ProductIds;
        CreatedAt= order.CreatedAt;
        UpdatedAt= order.UpdatedAt;
        Action = action;
    }

    public override string ToString()
    {
        return
            $"Id: {Id}, CustomerId:{CustomerId}, Quantity:{Quantity}, Total:{Total},  Status:{Status}, Address: {Address}, ProductIds: {ProductIds}, CreatedAt: {CreatedAt}" +
            $",  UpdatedAt: {UpdatedAt}";
    }
}