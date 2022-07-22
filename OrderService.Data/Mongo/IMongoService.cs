using MongoDB.Driver;
using OrderService.Entity.Models;

namespace OrderService.Data.Mongo;

public interface IMongoService
{
    IMongoCollection<Order> Orders { get; set; }
    IMongoCollection<Product> Products { get; set; }
}