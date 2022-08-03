using System.Text.Json;
using CustomerService.Application.Dto;
using CustomerService.Application.Interfaces;
using CustomerService.Application.Models;
using CustomerService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerService.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly ILogger<CustomerService> _logger;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerHelper _customerHelper;
    private readonly IPublisher _publisher;
    public CustomerService(ICustomerHelper customerHelper, ICustomerRepository customerRepository,ILogger<CustomerService> logger, IPublisher publisher)
    {
        _customerHelper = customerHelper;
        _customerRepository = customerRepository;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task<PagedList<Customer>> GetCustomersPaged(RequestParameters requestParameters, HttpContext context)
    {
        var customers = await _customerRepository.GetCustomersPaged(requestParameters);
        context.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(customers.MetaData));
        _logger.LogInformation("Getting all customers data's from database");
        return customers;
    }

    public async Task<Customer> GetById(string id)
    {
        var customer = await _customerRepository.GetWithId(id);
        _logger.LogInformation($"Getting customer data with {id} from database");
        return customer;
    }

    public async Task ValidateCustomer(string id)
    {
        await _customerRepository.GetWithId(id);
        _logger.LogInformation($"Validated customer with {id} from database");
    }

    public async Task<string> CreateCustomer(CustomerForCreationDto customerForCreation, HttpContext context)
    {
        var customer = customerForCreation.ToCustomer();
        await _customerRepository.CreateAsync(customer);
        context.Response.Headers.Add("location",
            $"https://{context.Request.Headers["Host"]}/api/Customers/{customer.Id}");
        var customerForLog = new CustomerForLogDto(customer, "Created");
        _publisher.PublishForLog(customerForLog);
        return customer.Id;
    }

    public async Task<Customer> UpdateCustomer(CustomerForUpdateDto customerForUpdate)
    {
        var customer = customerForUpdate.ToCustomer();
        await _customerHelper.SetCreatedAt(customer);
        await _customerRepository.UpdateAsync(customer);
        var customerForLog = new CustomerForLogDto(customer, "Updated");
        _publisher.PublishForLog(customerForLog);
        return customer;
    }

    public async Task DeleteCustomer(string id)
    {
        await _customerRepository.DeleteAsync(id);
        //await _publisher.SendDeleteCustomerRelatedOrdersCommand(id);
        _logger.LogInformation($"Customer with id {id} deleted.");
    }
}