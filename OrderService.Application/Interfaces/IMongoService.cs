using MongoDB.Driver;
using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IMongoService
{
    IMongoCollection<Order> Orders { get; set; }
    IMongoCollection<Product> Products { get; set; }
}