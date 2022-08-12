using CustomerService.Application.Exceptions;
using CustomerService.Application.Interfaces;
using CustomerService.Application.Models;
using CustomerService.Domain.Entities;
using GenericMongo.Bases;
using GenericMongo.Interfaces;
using MongoDB.Driver;

namespace CustomerService.Infrastructure.Repository;

public class CustomerRepository : RepositoryBase<Customer>,ICustomerRepository
{

    private readonly IMongoCollection<Customer> _collection;

    public CustomerRepository(IMongoService<Customer> mongoService) : base(mongoService)
    {
        _collection = mongoService.Collection;
        
    }

    public async Task<PagedList<Customer>> GetPaged(RequestParameters requestParameters)
    {
        var customers = await _collection
            .Search(requestParameters.SearchTerm,requestParameters.SearchArea)
            .CustomSort(requestParameters.OrderBy).ToListAsync();
        var returnCustomers = PagedList<Customer>.ToPagedList(customers,requestParameters.PageNumber,requestParameters.PageSize);
        if (returnCustomers.Any())
            return returnCustomers;
        throw new NotFoundException<Customer>();
    }
}