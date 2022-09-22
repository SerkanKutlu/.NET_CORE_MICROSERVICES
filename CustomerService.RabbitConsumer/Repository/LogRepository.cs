using CustomerService.RabbitConsumer.Models;
using GenericMongo.Bases;
using GenericMongo.Interfaces;

namespace CustomerService.RabbitConsumer.Repository;

public class LogRepository : RepositoryBase<Log>
{
    public LogRepository(IMongoService<Log> mongoService) : base(mongoService)
    {
    }
}