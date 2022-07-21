using CustomerService.Entity.Models;
using MongoDB.Driver;

namespace CustomerService.Data.Mongo;

public interface IMongoService
{
    IMongoCollection<Customer> Customers { get; set; }
}