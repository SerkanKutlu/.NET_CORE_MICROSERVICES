using GenericMongo.Bases;

namespace RedisSub.Entities;

public class Log : BaseEntity
{
    public string LogMessage { get; set; }
}