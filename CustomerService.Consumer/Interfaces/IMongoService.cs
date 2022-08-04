using CustomerService.Consumer.Models;
using MongoDB.Driver;

namespace CustomerService.Consumer.Interfaces;

public interface IMongoService
{
    IMongoCollection<Log> Logs { get; set; }
}