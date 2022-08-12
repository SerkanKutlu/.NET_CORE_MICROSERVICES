using CustomerService.Domain.ValueObjects;
using GenericMongo.Bases;

namespace CustomerService.Domain.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
}