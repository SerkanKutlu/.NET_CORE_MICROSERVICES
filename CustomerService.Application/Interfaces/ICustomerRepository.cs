using CustomerService.Application.Models;
using CustomerService.Domain.Entities;
using GenericMongo.Interfaces;

namespace CustomerService.Application.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    // Task CreateAsync(Customer newCustomer);
    // Task UpdateAsync(Customer updatedCustomer);
    // Task DeleteAsync(string customerId);
    Task<PagedList<Customer>> GetPaged(RequestParameters requestParameters);
    // Task<Customer> GetWithId(string customerId);
    //Task Validate(string customerId);
}