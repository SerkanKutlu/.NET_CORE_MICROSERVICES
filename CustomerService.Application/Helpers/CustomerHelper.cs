using CustomerService.Application.Interfaces;
using CustomerService.Domain.Entities;

namespace CustomerService.Application.Helpers;

public class CustomerHelper : ICustomerHelper
{
    private readonly ICustomerRepository _customerRepository;
    public CustomerHelper(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task SetCreatedAt(Customer customerForUpdate)
    {
        var oldCustomer = await _customerRepository.GetWithId(customerForUpdate.Id);
        customerForUpdate.CreatedAt = oldCustomer.CreatedAt;
    }
}