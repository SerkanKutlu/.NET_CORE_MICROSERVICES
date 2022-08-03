using CustomerService.Domain.ValueObjects;

namespace CustomerService.Application.Dto;

public class CustomerForLogDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Action { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Name:{Name}, Email:{Email}, Address: {Address}, Created: {CreatedAt}, Updated:{UpdatedAt}";
    }
}