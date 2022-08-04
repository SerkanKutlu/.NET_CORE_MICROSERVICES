using MongoDB.Driver;
using OrderService.Consumer.Models;

namespace OrderService.Consumer.Interfaces;

public interface IMongoService
{
    IMongoCollection<Log> Logs { get; set; }
}