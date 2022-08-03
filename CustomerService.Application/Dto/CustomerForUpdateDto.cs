using CustomerService.Domain.Entities;
using CustomerService.Domain.ValueObjects;

namespace CustomerService.Application.Dto;

public class CustomerForUpdateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    
    public Customer ToCustomer()
    {
        return new Customer
        {
            Address = Address,
            Email = Email.ToLowerInvariant(),
            UpdatedAt = DateTime.UtcNow,
            Id = Id,
            Name = Name
        };
    }
}