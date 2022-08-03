using CustomerService.Application.Exceptions;
using CustomerService.Application.Interfaces;
using CustomerService.Application.Models;
using CustomerService.Domain.Entities;
using MongoDB.Driver;

namespace CustomerService.Infrastructure.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly IMongoCollection<Customer> _customers;
    public CustomerRepository(IMongoService mongoService)
    {
        _customers = mongoService.Customers;
    }
    
    
    public async Task CreateAsync(Customer newCustomer)
    {
        await _customers.InsertOneAsync(newCustomer);
    }

      
    public async Task UpdateAsync(Customer updatedCustomer)
    {
        var result = await _customers.ReplaceOneAsync(c => c.Id == updatedCustomer.Id, updatedCustomer);
        if (!result.IsModifiedCountAvailable && result.ModifiedCount == 0)
            throw new NotFoundException<Customer>(updatedCustomer.Id);
    }

        
    public async Task DeleteAsync(string customerId)
    {
        var result = await _customers.DeleteOneAsync(c => c.Id == customerId);
        if (result.DeletedCount==0)
            throw new NotFoundException<Customer>(customerId);
    }

        
    public async Task<PagedList<Customer>> GetCustomersPaged(RequestParameters requestParameters)
    {
        var customers = await _customers
            .Search(requestParameters.SearchTerm)
            .CustomSort(requestParameters.OrderBy).ToListAsync();
        var returnCustomers = PagedList<Customer>.ToPagedList(customers,requestParameters.PageNumber,requestParameters.PageSize);
        if (returnCustomers.Any())
            return returnCustomers;
        throw new NotFoundException<Customer>();
    }

        
    public async Task<Customer> GetWithId(string customerId)
    {
        var customer = await _customers.Find(c => c.Id == customerId).FirstOrDefaultAsync();
        if(customer == null)
            throw new NotFoundException<Customer>(customerId);
        return customer;

    }

        
    public async Task Validate(string customerId)
    {
        var customer = await _customers.Find(c => c.Id == customerId).FirstOrDefaultAsync();
        if(customer==null)
            throw new NotFoundException<Customer>(customerId);
    }
}