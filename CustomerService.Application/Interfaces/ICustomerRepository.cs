using CustomerService.Application.Models;
using CustomerService.Domain.Entities;

namespace CustomerService.Application.Interfaces;

public interface ICustomerRepository
{
    Task CreateAsync(Customer newCustomer);
    Task UpdateAsync(Customer updatedCustomer);
    Task DeleteAsync(string customerId);
    Task<PagedList<Customer>> GetCustomersPaged(RequestParameters requestParameters);
    Task<Customer> GetWithId(string customerId);
    Task Validate(string customerId);
}