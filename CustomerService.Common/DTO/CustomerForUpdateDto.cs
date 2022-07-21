using CustomerService.Entity.Models;

namespace CustomerService.Common.DTO;

public class CustomerForUpdateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
}