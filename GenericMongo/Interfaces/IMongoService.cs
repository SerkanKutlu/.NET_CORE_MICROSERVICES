using GenericMongo.Bases;
using MongoDB.Driver;

namespace GenericMongo.Interfaces;

public interface IMongoService<T> where T:BaseEntity
{
    public IMongoCollection<T> Collection { get; set; }
}