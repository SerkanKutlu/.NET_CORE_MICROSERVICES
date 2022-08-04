using CustomerService.Consumer.Interfaces;
using CustomerService.Consumer.Models;
using MongoDB.Driver;

namespace CustomerService.Consumer.Services;

public class ConsumerService:IConsumerService
{

    private readonly IMongoCollection<Log> _logs;

    public ConsumerService(IMongoService mongoService)
    {
        _logs = mongoService.Logs;
    }


    public void LogMany(IEnumerable<Log> logs)
    {
       _logs.InsertMany(logs);
    }
}