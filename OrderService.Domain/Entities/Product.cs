using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.Domain.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ImageUrl { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}