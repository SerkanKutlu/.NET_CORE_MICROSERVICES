using CustomerService.Domain.Entities;
using CustomerService.Domain.ValueObjects;

namespace CustomerService.Application.Dto;

public class CustomerForCreationDto
{
   
    
    public string Name { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }

    public Customer ToCustomer()
    {
        return new Customer
        {
            Address = Address,
            Email = Email.ToLowerInvariant(),
            UpdatedAt = DateTime.MinValue,
            CreatedAt = DateTime.UtcNow,
            Id = Guid.NewGuid().ToString(),
            Name = Name.ToLowerInvariant()
        };
    }
    
}