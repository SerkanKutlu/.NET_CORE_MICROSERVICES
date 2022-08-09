
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using CustomerService.Domain.Entities;
using CustomerService.Domain.ValueObjects;

namespace CustomerService.Application.Dto;

public class CustomerForLogDto : ISerializer<CustomerForLogDto>, IDeserializer<CustomerForLogDto>
{
    public string Id { get; set; }
    public string Name { get;set; }
    public string Email { get;set; }
    public Address Address { get; set; }
    public DateTime CreatedAt { get;set; }
    public DateTime UpdatedAt { get; set;}
    public string Action { get; set;}


    public byte[] Serialize(CustomerForLogDto data, SerializationContext context)
    {
        var serialized = JsonSerializer.Serialize(data);
        return Encoding.UTF8.GetBytes(serialized);
    }

    public CustomerForLogDto Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var stringDto = Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<CustomerForLogDto>(stringDto);
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name:{Name}, Email:{Email}, Address: {Address}, Created: {CreatedAt}, Updated:{UpdatedAt}";
    }

   

    public void FillWithCustomer(Customer customer, string action)
    {
        Address = customer.Address;
        Email = customer.Email;
        CreatedAt = customer.CreatedAt;
        UpdatedAt = customer.UpdatedAt;
        Id = customer.Id;
        Name = customer.Name;
        Action = action;
    }

 
}