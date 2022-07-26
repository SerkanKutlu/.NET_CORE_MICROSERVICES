using MongoDB.Driver;
using OrderService.Domain.Entities;
using OrderService.Domain.ValueObjects;

namespace OrderService.Infrastructure.Mongo;

public static class SeedOrderData
{
    public static void SeedData(IMongoCollection<Order> orders)
    {
        var orderList = orders.Find(c => true).Limit(1).ToList();
        if (!orderList.Any())
        {
            orders.InsertMany(GetInitialOrders());
        }
        
    }
    private static IEnumerable<Order>  GetInitialOrders()
    {
        
        return new[]
        {
            new Order
            {
                Id = "62d91541a2a411f44df899cf",
                CustomerId = "62d91541a2a411f44df899ce",
                Address = new Address
                {
                    AddressLine = "cumhuriyet",
                    City = "samsun",
                    CityCode = 55,
                    Country = "tr",
                },
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                Quantity = 1,
                Status = "delivered",
                Total = 5000f,
                ProductIds = {"32d91541a2a411f44df899cf"}
            }
        };
    }
}