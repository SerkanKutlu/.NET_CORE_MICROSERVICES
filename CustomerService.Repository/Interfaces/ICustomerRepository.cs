using CustomerService.Entity.Models;

namespace CustomerService.Repository.Interfaces;

public interface ICustomerRepository
{
    Task CreateAsync(Customer newCustomer);
    Task UpdateAsync(Customer updatedCustomer);
    Task DeleteAsync(string customerId);
    Task<IEnumerable<Customer>> GetAll(RequestParameters requestParameters);
    Task<Customer> GetWithId(string customerId);
    Task Validate(string customerId);
}