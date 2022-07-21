using CustomerService.Common.Exceptions;
using CustomerService.Data.Mongo;
using CustomerService.Entity.Models;
using CustomerService.Repository.Extensions;
using MongoDB.Driver;

namespace CustomerService.Repository.Interfaces;

public class CustomerRepository : ICustomerRepository
{
    private readonly IMongoService _mongoService;
    public CustomerRepository(IMongoService mongoService)
    {
        _mongoService = mongoService;
          
    }
    
    
    public async Task CreateAsync(Customer newCustomer)
    {
        await _mongoService.Customers.InsertOneAsync(newCustomer);
    }

      
    public async Task UpdateAsync(Customer updatedCustomer)
    {
        var result = await _mongoService.Customers.ReplaceOneAsync(c => c.Id == updatedCustomer.Id, updatedCustomer);
        if (!result.IsModifiedCountAvailable && result.ModifiedCount == 0)
            throw new NotFoundException<Customer>(updatedCustomer.Id);
    }

        
    public async Task DeleteAsync(string customerId)
    {
        var result = await _mongoService.Customers.DeleteOneAsync(c => c.Id == customerId);
        if (result.DeletedCount==0)
            throw new NotFoundException<Customer>(customerId);
    }

        
    public async Task<IEnumerable<Customer>> GetAll(RequestParameters requestParameters)
    {
        var customers= await _mongoService.Customers.
             Search(requestParameters.SearchTerm)
            .CustomSort(requestParameters.OrderBy)
            .Skip((requestParameters.PageNumber - 1) * requestParameters.PageSize)
            .Limit(requestParameters.PageSize)
            .ToListAsync();
        if (customers.Any())
            return customers;
        throw new NotFoundException<Customer>();
    }

        
    public async Task<Customer> GetWithId(string customerId)
    {
        var customer = await _mongoService.Customers.Find(c => c.Id == customerId).FirstOrDefaultAsync();
        if(customer == null)
            throw new NotFoundException<Customer>(customerId);
        return customer;

    }

        
    public async Task Validate(string customerId)
    {
        var customer = await _mongoService.Customers.Find(c => c.Id == customerId).FirstOrDefaultAsync();
        if(customer==null)
            throw new NotFoundException<Customer>(customerId);
    }
}