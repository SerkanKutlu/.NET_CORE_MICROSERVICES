using CustomerService.Application.Dto;
using CustomerService.Application.Models;
using CustomerService.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CustomerService.Application.Interfaces;

public interface ICustomerRequestService
{
    Task<PagedList<Customer>> GetAllCustomers(RequestParameters requestParameters,HttpContext context);
    Task<Customer> GetById(string id);
    Task ValidateCustomer(string id);
    Task<string> CreateCustomer(CustomerForCreationDto customerForCreation,HttpContext context);
    Task<Customer> UpdateCustomer(CustomerForUpdateDto customerForCreation);
    Task DeleteCustomer(string id);
}