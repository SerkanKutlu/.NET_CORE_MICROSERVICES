using System.Text.Json;
using CustomerService.Application.Dto;
using CustomerService.Application.Events;
using CustomerService.Application.Exceptions;
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
    private readonly IRedisPublisher _redisPublisher;
    private readonly IKafkaPublisher _kafkaPublisher;
    private readonly IRabbitPublisher _rabbitPublisher;
    public CustomerService(ICustomerHelper customerHelper, ICustomerRepository customerRepository,ILogger<CustomerService> logger,IRedisPublisher redisPublisher, IKafkaPublisher kafkaPublisher, IRabbitPublisher rabbitPublisher)
    {
        _customerHelper = customerHelper;
        _customerRepository = customerRepository;
        _logger = logger;
        _redisPublisher = redisPublisher;
        _kafkaPublisher = kafkaPublisher;
        _rabbitPublisher = rabbitPublisher;
    }

    public async Task<PagedList<Customer>> GetPagedCustomers(RequestParameters requestParameters, HttpContext context)
    {
        var customers = await _customerRepository.GetPaged(requestParameters);
        if (!customers.Any())
        {
            throw new NotFoundException<Customer>();
            
        }
        context.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(customers.MetaData));
        _logger.LogInformation("Getting all customers data's from database");
        return customers;
    }

    public async Task<Customer> GetById(string id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new NotFoundException<Customer>();
        _logger.LogInformation($"Getting customer data with {id} from database");
        return customer;
    }

    public async Task ValidateCustomer(string id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new NotFoundException<Customer>();
        _logger.LogInformation($"Validated customer with {id} from database");
    }

    public async Task<string> CreateCustomer(CustomerForCreationDto customerForCreation, HttpContext context)
    {
        var customer = customerForCreation.ToCustomer();
        await _customerRepository.AddAsync(customer);
        context.Response.Headers.Add("location",
            $"https://{context.Request.Headers["Host"]}/api/Customers/{customer.Id}");
        var customerCreated = new CustomerCreated();
        customerCreated.FillWithCustomer(customer);
        await _kafkaPublisher.Publish(customerCreated);
        await _redisPublisher.Publish(customerCreated);
        _rabbitPublisher.Publish(customerCreated);
        return customer.Id;
    }

    public async Task<Customer> UpdateCustomer(CustomerForUpdateDto customerForUpdate)
    {
        var customer = customerForUpdate.ToCustomer();
        await _customerHelper.SetCreatedAt(customer);
        var result = await _customerRepository.UpdateAsync(customer);
        if (result == null)
        {
            throw new NotFoundException<Customer>(customerForUpdate.Id);
        }
        return customer;
    }

    public async Task DeleteCustomer(string id)
    {
        var result  = await _customerRepository.DeleteAsync(id);
        if (result == null)
        {
            throw new NotFoundException<Customer>(id);
        }
        _logger.LogInformation($"Customer with id {id} deleted.");
    }
    
}