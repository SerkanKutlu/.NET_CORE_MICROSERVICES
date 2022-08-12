using CustomerService.Application.Exceptions;
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
        var oldCustomer = await _customerRepository.GetByIdAsync(customerForUpdate.Id);
        if (oldCustomer == null)
        {
            throw new NotFoundException<Customer>();
        }
        customerForUpdate.CreatedAt = oldCustomer.CreatedAt;
    }
}