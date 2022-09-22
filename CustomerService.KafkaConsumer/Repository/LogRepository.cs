using CustomerService.KafkaConsumer.Models;
using GenericMongo.Bases;
using GenericMongo.Interfaces;

namespace CustomerService.KafkaConsumer.Repository;

public class LogRepository : RepositoryBase<Log>
{
    public LogRepository(IMongoService<Log> mongoService) : base(mongoService)
    {
    }
}