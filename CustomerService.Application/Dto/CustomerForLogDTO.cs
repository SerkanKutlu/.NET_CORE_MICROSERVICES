using CustomerService.Domain.Entities;
using CustomerService.Domain.ValueObjects;

namespace CustomerService.Application.Dto;

public class CustomerForLogDto
{
    public string Id { get; set; }
    public string Name { get;set; }
    public string Email { get;set; }
    public Address Address { get; set; }
    public DateTime CreatedAt { get;set; }
    public DateTime UpdatedAt { get; set;}
    public string Action { get; set;}
    

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