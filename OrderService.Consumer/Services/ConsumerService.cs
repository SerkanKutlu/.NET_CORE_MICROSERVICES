using MongoDB.Driver;
using OrderService.Consumer.Interfaces;
using OrderService.Consumer.Models;

namespace OrderService.Consumer.Services;

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