using CustomerService.Common.Models;
using CustomerService.Entity.Models;

namespace CustomerService.Repository.Repository;

public interface ICustomerRepository
{
    Task CreateAsync(Customer newCustomer);
    Task UpdateAsync(Customer updatedCustomer);
    Task DeleteAsync(string customerId);
    Task<PagedList<Customer>> GetAll(RequestParameters requestParameters);
    Task<Customer> GetWithId(string customerId);
    Task Validate(string customerId);
}