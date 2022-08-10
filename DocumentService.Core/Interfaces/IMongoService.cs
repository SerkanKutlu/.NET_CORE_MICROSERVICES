using Core.Entity;
using MongoDB.Driver;

namespace Core.Interfaces;

public interface IMongoService
{
    IMongoCollection<Document> Documents { get; set; }
}