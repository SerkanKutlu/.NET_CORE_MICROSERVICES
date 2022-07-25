using CustomerService.Domain.Entities;
using MongoDB.Driver;

namespace CustomerService.Application.Interfaces;

public interface IMongoService
{
    IMongoCollection<Customer> Customers { get; set; }
}