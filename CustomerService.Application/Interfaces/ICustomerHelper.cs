using CustomerService.Domain.Entities;

namespace CustomerService.Application.Interfaces;

public interface ICustomerHelper
{
    Task SetCreatedAt(Customer customerForUpdate);
}