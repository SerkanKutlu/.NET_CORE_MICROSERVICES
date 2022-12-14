using GenericMongo.Bases;
using GenericMongo.Interfaces;
using RedisSub.Models;

namespace RedisSub.Repositories;

public class LogRepository : RepositoryBase<Log>
{
    public LogRepository(IMongoService<Log> mongoService) : base(mongoService)
    {
    }
}