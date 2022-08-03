using CustomerService.Domain.Entities;
using CustomerService.Domain.ValueObjects;

namespace CustomerService.Application.Dto;

public class CustomerForLogDto
{
    private string Id { get; }
    private string Name { get; }
    private string Email { get; }
    private Address Address { get;  }
    private DateTime CreatedAt { get; }
    private DateTime UpdatedAt { get; }
    public string Action { get; }

    public CustomerForLogDto(Customer customer,string action)
    {
        Address = customer.Address;
        Email = customer.Email;
        CreatedAt = customer.CreatedAt;
        UpdatedAt = customer.UpdatedAt;
        Id = customer.Id;
        Name = customer.Name;
        Action = action;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name:{Name}, Email:{Email}, Address: {Address}, Created: {CreatedAt}, Updated:{UpdatedAt}";
    }
    
    
}